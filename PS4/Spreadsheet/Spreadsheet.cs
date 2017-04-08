using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpreadsheetUtilities;
using System.Xml;
using System.IO;


namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, object> ss; //The Spreadsheet containing cells and their contents, which can be a string, double or Formula.
        private DependencyGraph d; //Storage for dependencies        
        private string f; //File path
        private bool changed;
        private Dictionary<string, object> values;

        /// <summary>
        /// Default spreadsheet constructor creating an empty spreadsheet.
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "Default")
        {
            Changed = false;
            ss = new Dictionary<string, object>();
            values = new Dictionary<string, object>();
            d = new DependencyGraph();
            Version = "Default";
        }

        /// <summary>
        /// Spreadsheet constructor method with given parameters for validation, normalization and versioning
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version): base(isValid, normalize, version)
        {
            Changed = false;
            Version = version;
            ss = new Dictionary<string, object>();
            values = new Dictionary<string, object>();
            d = new DependencyGraph();
        }

        /// <summary>
        /// Spreadsheet constructor method with given parameters for spreadsheet file path, validation, normalization and versioning
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string file, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            Changed = false;
            Version = version;
            f = file;
            ss = new Dictionary<string, object>();
            values = new Dictionary<string, object>();
            d = new DependencyGraph();
            XmlReader reader;

            try
            {
                using (reader = XmlReader.Create(file))
                {
                    string cell = "";
                    while (reader.Read())
                    {
                        
                        if (reader.IsStartElement())
                            switch(reader.Name)
                            {
                                case "spreadsheet":
                                    if (reader.GetAttribute("version") != version)
                                        throw new SpreadsheetReadWriteException("Version Mismatch"); //Version check
                                    break;
                                case "name":
                                    reader.Read();
                                    cell = reader.Value; //The cell name
                                    break;
                                case "contents":
                                    double num;
                                    reader.Read();
                                    char c = reader.Value.First();
                                    /*if (c.Equals('='))
                                        ss[cell] = new Formula(reader.Value.Substring(1,reader.Value.Length-1));*/
                                    if (Double.TryParse(reader.Value, out num))
                                        ss[cell] = num;
                                    else
                                        ss[cell] = reader.Value; //Add content to cell in spreadsheet dictionary
                                    break;
                                case "value":
                                    reader.Read();
                                    values[cell] = reader.Value; //Add value to dictionary
                                    break;
                                case "dependency":
                                    reader.Read();
                                    d.AddDependency(reader.Value.ToString().Split(',')[0], reader.Value.ToString().Split(',')[1]); //Load dependencies
                                    break;
                            }
                    }
                }
            }
            catch(FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File Not Found");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            object v;
         
                if (name == null || IsValid(Normalize(name)) == false || isValidName(Normalize(name)) == false)
                    throw new InvalidNameException();
            
            if (ss.TryGetValue(Normalize(name), out v) == false) //If the name cell is empty return the empty string.
                return "";
            else return v;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return ss.Keys; //Everything in the dictionary is a "non-empty cell"
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.  (No change is made to the spreadsheet.)
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> dependees = new HashSet<string>();
            Func<string, double> varlook = EvaluateFormula;

            if (Object.Equals(formula, null))
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(name) || isValidName(name) == false)
                throw new InvalidNameException();

            foreach (string a in formula.GetVariables())
                d.AddDependency(name, a);

            dependees.Add(name);
            foreach (string s in GetCellsToRecalculate(Normalize(name)))
                dependees.Add(s);

            ss[name] = formula;
            values[name] = formula.Evaluate(varlook);

            return dependees;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> dependees = new HashSet<string>();
            if (text == null)
                throw new ArgumentNullException();
            if (String.IsNullOrEmpty(name) || isValidName(name) == false)
                throw new InvalidNameException();
            if (string.IsNullOrEmpty(text))
                return dependees;
            dependees.Add(name);
            foreach (string s in GetCellsToRecalculate(name))
                dependees.Add(s);

            ss[name] = text;
            values[name] = text;

            return dependees;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> dependees = new HashSet<string>();
            if (String.IsNullOrEmpty(name) || isValidName(name) == false)
                throw new InvalidNameException();
            dependees.Add(name);
            foreach (string s in GetCellsToRecalculate(name))
                dependees.Add(s);
            ss[name] = number;
            values[name] = number;

            return dependees;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name.Equals(null))
                throw new ArgumentNullException();
            if (isValidName(name) == false)
                throw new InvalidNameException();
            return d.GetDependees(name);
        }

        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override string GetSavedVersion(string filename)
        {
            XmlReader reader;
            try
            {
                using(reader = XmlReader.Create(filename))
                {
                    return reader.GetAttribute("version");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new SpreadsheetReadWriteException("File cannot be accessed.");
            }
            catch (FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("File does not exist.");
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {            
            if (Changed == false)
            {
                return;
            }
            
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach(string a in GetNamesOfAllNonemptyCells()) //For every cell create a new cell element of the save file
                    {
                        writer.WriteStartElement("cell");
                        
                        writer.WriteElementString("name", a.ToString());
                        if(ss[a].GetType() == typeof(Formula))
                            writer.WriteElementString("contents", "=" + ss[a].ToString());
                        else if(ss[a].GetType() == typeof(double))
                            writer.WriteElementString("contents", ss[a].ToString());
                        else
                            writer.WriteElementString("contents", ss[a].ToString());
                        writer.WriteElementString("value", values[a].ToString());
                        foreach (string s in d.GetDependents(a))
                            writer.WriteElementString("dependency", a + "," + s);
                        writer.WriteEndElement();
                    }
                   

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                Changed = false;
            }
            catch(UnauthorizedAccessException)
            {
                throw new SpreadsheetReadWriteException("Location cannot be accessed.");
            }
            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("File could not be read");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            Func<string, double> varlook = EvaluateFormula;
            if (IsValid(name) == false || isValidName(name) == false || name == null)
                throw new InvalidNameException();
            try
            {
                return values[name];
            }
            catch(KeyNotFoundException)
            {
                return "";
            }
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            double d;
            if (content == null)
                throw new ArgumentNullException("No Content");
            else if (name == null || IsValid(name) == false || isValidName(name) == false)
                throw new InvalidNameException();
            else if (string.IsNullOrEmpty(content))
            {
                return SetCellContents(name, content); //SetCellContents for empty strings
            }
            else if (Double.TryParse(content, out d))
            {
                Changed = true;
                return SetCellContents(name, d); //SetCellContents for doubles
            }
            else if (content.First().Equals('='))
            {
                Changed = true;
                try
                {
                    return SetCellContents(name, new Formula(content.Substring(1, content.Length - 1))); //SetCellContents for Formulae
                }
                catch
                {
                    return SetCellContents(name, "=");
                }
            }
            else
            {
                Changed = true;
                return SetCellContents(name, content); //SetCellContents for normal text
            }
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }

            protected set
            {
                changed = value;          
            }
        }

        //////////////////////////////Helpers/////////////////////////////////////////////////////////
        /// <summary>
        /// Helper method to check the validity of cell names.
        /// As the AbstractSpreadsheet class specifies:
        /// A string is a valid cell name if and only if:
        ///   (1) its first character is an underscore or a letter
        ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
        /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isValidName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            if (Char.IsLetter(name.First()) && Char.IsDigit(name.Last())) //Check if first character is letter
            {
                foreach (char c in name.Substring(1,name.Length-2))
                {
                    if (char.IsLetterOrDigit(c) == false) //check if the rest are letters or numbers
                        return false;
                }
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Evaluate formula for lookup delegate
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private double EvaluateFormula(string s)
        {
            object o;
            if (values.TryGetValue(s, out o) == true && values[s].GetType() == typeof(double))
                return (double)values[s];
            return 0;
        }
    }
}