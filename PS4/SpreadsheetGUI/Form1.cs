using Newtonsoft.Json;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class Form1 : Form
    {
        //Helper members for displaying and changing cell information in the GUI
        private string[] columnNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private int column;
        private int row;
        private string content;
        private string value;
        private string cellName;

        private BackgroundWorker worker;
        SocketState spreadsheetState;
        Spreadsheet ss;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_Closing;
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

            worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(DoWork);

            // This timer simulates updates coming from the server
            Timer frameTimer = new Timer();
            frameTimer.Interval = 33;
            frameTimer.Tick += UpdateFrame;
            frameTimer.Start();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            //Connect to the server
            StaticNetworking.ConnectToServer(FirstContact, IPTextBox.Text);
        }

        /// <summary>
        /// Everything that occurs when the spreadsheet panel first loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {
            spreadsheetPanel1.GetSelection(out column, out row);
            spreadsheetPanel1.GetValue(column, row, out content);
            int rowDisplay = row + 1;
            CellNameTextBox.Text = cellName = columnNames[column] + rowDisplay;
            value = ss.GetCellValue(cellName).ToString();

            CellContentTextBox.Text = content;
            spreadsheetPanel1.SelectionChanged += ChangeCellBoxDetails;
            spreadsheetPanel1.SelectionChanged += SetFocusToContentInput;
        }

        /// <summary>
        /// Operations occuring when a new cell is selected.
        /// </summary>
        /// <param name="s"></param>
        void ChangeCellBoxDetails(SpreadsheetPanel s)
        {
            //Get the address for the selected cell
            spreadsheetPanel1.GetSelection(out column, out row);
            spreadsheetPanel1.GetValue(column, row, out content);
            int rowDisplay = row + 1;
            CellNameTextBox.Text = cellName = columnNames[column] + rowDisplay;
            value = ss.GetCellValue(cellName).ToString();

            //Change the textboxes above the spreadsheet panel
            if (ss.GetCellContents(cellName).GetType() == typeof(Formula))
                CellContentTextBox.Text = "=" + ss.GetCellContents(cellName);
            else
                CellContentTextBox.Text = ss.GetCellContents(cellName).ToString();
            CellValueTextBox.Text = value;
        }

        void SetFocusToContentInput(SpreadsheetPanel s)
        {
            CellContentTextBox.Focus();
        }

        private void applicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("--File Menu Items--\n\nNew Spreadsheet: Opens window with an empty, brand new spreadsheet.\n" +
                "Save: Saves the spreadsheet to disk.\n" +
                "Open: Opens a spreadsheet previously saved on disk.\n" +
                "Close: Closes the application.\n\n" +
                "--Additional Features--\n\n" +
                "- When the file has been modified without saving, an asterisk will\nshow up next to the filename in the form header.\n\n" +
                "- When a save is completed, the Form's header/name will change to the filename you saved the spreadsheet as.\n\n" +
                "-- Basic Controls --\n\n" +
                "Select a Cell: Simply click your mouse on any of the cells to select it.\n\n" +
                "Change Value/Contents of a Cell: In the text box labeled input, enter a number, string, or formula and then click enter");
        }

        /// <summary>
        /// When the input button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentChangeButton_Click(object sender, EventArgs e)
        {
            content = CellContentTextBox.Text;
            ISet<string> cellsToUpdate = ss.SetContentsOfCell(cellName, content);
            value = ss.GetCellValue(cellName).ToString();
            spreadsheetPanel1.SetValue(column, row, value);
            CellValueTextBox.Text = value;

            //Adds asterisk to the filename in the form header if the file was changed
            //to note a need to save the file.
            if (ss.Changed == true)
            {
                if (!this.Text.Contains("*"))
                {
                    this.Text = this.Text + "*";
                }
            }

            //Update cell views based on dependencies
            UpdateCellsWithCorrectValue(cellsToUpdate);
        }

        /// <summary>
        /// Handles the enter button being pressed when there is text in the input
        /// box for opening a file or creating new spreadsheet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPressHandler(object sender, KeyPressEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void newSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Create a brand new blank spreadsheet form
            Form1 newSpreadsheet = new Form1();
            newSpreadsheet.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set dialog properties
            saveFileDialog1.DefaultExt = "sprd";
            saveFileDialog1.Filter = "Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;

            //Show the actual dialog
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    if (!saveFileDialog1.FileName.Equals(""))
                    {
                        if (ss.Changed == true)
                        {
                            ss.Save(saveFileDialog1.FileName); //Saves the spreadsheet's state in the spreadsheet
                            this.Text = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf('\\') + 1);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error: Could not save file to disk.");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 opened = new Form1();

            //Set dialog properties
            openFileDialog1.DefaultExt = "sprd";
            openFileDialog1.Filter = "Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            //Show the actual dialog
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                openFileDialog1.Filter = "Spreadsheets (*.sprd)|*.sprd|All Files (*.*)|*.*";

                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                    opened.ss = new Spreadsheet(openFileDialog1.FileName, s => true, s => s.ToUpper(), "ps6");
                    opened.Text = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf('\\') + 1);
                    opened.UpdateCellsWithCorrectValue(opened.ss.GetNamesOfAllNonemptyCells());
                    opened.Show();
                }
                catch
                {
                    MessageBox.Show("Error: Could not read file from disk.");
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If the file is changed without saving, and the close button is clicked...
            //Ask the user if they want to save their changes.
            if (ss.Changed == true)
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes you have made?", "", MessageBoxButtons.YesNo);

                //Yes button on dialog selected
                if (result == DialogResult.Yes)
                {
                    saveFileDialog1.DefaultExt = "sprd";
                    saveFileDialog1.Filter = "Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    DialogResult saveResult = saveFileDialog1.ShowDialog();
                    if (saveResult == DialogResult.OK)
                    {
                        try
                        {

                            if (!saveFileDialog1.FileName.Equals(""))
                            {
                                ss.Save(saveFileDialog1.FileName); //Saves the spreadsheet's state in the spreadsheet
                                this.Text = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf('\\') + 1); ;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error: Could not save file to disk.");
                        }
                    }
                    else if (result == DialogResult.No) //No button on dialog selected
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ss.Changed == true)
            {
                DialogResult result = MessageBox.Show("Do you want to save the changes you have made?", "", MessageBoxButtons.YesNo);

                //Yes button on dialog selected
                if (result == DialogResult.Yes)
                {
                    saveFileDialog1.DefaultExt = "sprd";
                    saveFileDialog1.Filter = "Spreadsheet files (*.sprd)|*.sprd|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    DialogResult saveResult = saveFileDialog1.ShowDialog();
                    if (saveResult == DialogResult.OK)
                    {
                        try
                        {

                            if (!saveFileDialog1.FileName.Equals(""))
                            {
                                ss.Save(saveFileDialog1.FileName); //Saves the spreadsheet's state in the spreadsheet
                                this.Text = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf('\\') + 1); ;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Error: Could not save file to disk.");
                        }
                    }
                    else if (result == DialogResult.No) //No button on dialog selected
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Helper method that uses the passed in IEnumerable to update each of those
        /// Cell's values in the view of the spreadsheet.
        /// </summary>
        /// <param name="cellsToUpdate"></param>
        private void UpdateCellsWithCorrectValue(IEnumerable<string> cellsToUpdate)
        {

            Regex re = new Regex(@"([a-zA-Z]+)(\d+)");
            Match splitString;
            string letterPart;
            string numberPart;
            int num;
            string value;

            //Gets all the non empty cell names from the back end spreadsheet and populates
            //the correct cells in the viewable form of the spreadsheet.
            foreach (string cellName in cellsToUpdate)
            {
                splitString = re.Match(cellName);

                letterPart = splitString.Groups[1].Value;
                numberPart = splitString.Groups[2].Value;

                Int32.TryParse(numberPart, out num);

                column = columnNames.ToList().IndexOf(letterPart);
                row = num - 1;

                //Evaluate the cell formula again
                if (ss.GetCellContents(cellName).GetType() == typeof(Formula))
                    ss.SetContentsOfCell(cellName, "=" + ss.GetCellContents(cellName).ToString());
                value = ss.GetCellValue(cellName).ToString();

                spreadsheetPanel1.SetValue(column, row, value);
            }
        }

        /// <summary>
        /// Updates the GUI whenever messages from the server comes in.
        /// *** DO WE NEED TO REGISTER THIS EVENT? ***
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateFrame(object sender, EventArgs e)
        {
            // Update GUI based on changes made by server from previous message
            ContentChangeButton_Click(sender, e);
        }

        /// <summary>
        /// Event handler for when the connect button is clicked in the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (!UsernameTextBox.Text.Equals("") && !IPTextBox.Text.Equals(""))
            {
                UsernameTextBox.Enabled = false;
                IPTextBox.Enabled = false;
                ConnectButton.Enabled = false;

                //Begin server connection work
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("You must enter a server name and a player name to connect.");
            }
        }

        private void IPLabel_Click(object sender, EventArgs e)
        {
            IPTextBox.Focus();
        }

        private void UsernameLabel_Click(object sender, EventArgs e)
        {
            UsernameTextBox.Focus();
        }

        private void IPTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        // SERVER CONNECTION WORK BEGINS HERE

        /// <summary>
        /// Initial handshake with the server
        /// </summary>
        /// <param name="state"></param>
        public void FirstContact(SocketState state)
        {
            state.callMe = ReceiveStartup;
            StaticNetworking.Send(state.Socket, UsernameTextBox.Text + "\n");
        }

        /// <summary>
        /// Receive the startup information from the server and process.
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveStartup(SocketState state)
        {
            state.callMe = ReceiveInitialSpreadsheet;
            StaticNetworking.GetData(state);
            spreadsheetState = state;

            // Retrieve string message from state
            Byte[] b = state.MessageBuffer;
            string[] s = state.SB.ToString().Split('\t');

            // Lock the spreadsheet while data processes
            lock (spreadsheetState)
            {
                // Process startup data for requesting file from server
                openOrCreateFile(b, s);
            }

            // Clear the stringbuilder for the next round of messages from the server
            state.SB.Clear();
        }

        private void openOrCreateFile(Byte[] b, string[] s)
        {/*
            MemoryStream stream = new MemoryStream(b);
            StreamReader reader = new StreamReader(stream);

            using (reader)
            {
                string line;
                while (reader.Peek() >= 0)
                {
                    try
                    {
                        line = reader.ReadLine();
                        NetworkInfoLabel.Text = NetworkInfoLabel.Text + line; 
                    }
                    catch (Exception)
                    {}
                }
            } */

            for(int i = 0; i < s.Length; i++)
            {
                NetworkInfoLabel.Text = NetworkInfoLabel.Text + s[i];
            }
        }

        /// <summary>
        /// Receive the initial version of the loaded or new "world" (spreadsheet)
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveInitialSpreadsheet(SocketState state)
        {
            Byte[] b = state.MessageBuffer;
            string[] s = state.SB.ToString().Split('\t');
            spreadsheetState = state;

            lock(spreadsheetState)
            {
                ProcessSpreadsheetLoad(b, s);
            }

            // Clear stringbuilder for next time through so no repeated data is processed
            state.SB.Clear();
            state.callMe = ReceiveSpreadsheetUpdates;
        }

        public void ProcessSpreadsheetLoad(Byte[] b, string[] s)
        {
            MemoryStream stream = new MemoryStream(b);
            StreamReader reader = new StreamReader(stream);
            Dictionary<string, string> spreadsheetData = new Dictionary<string, string>();

            // Begin reading the data stream
            using (reader)
            {
                try
                {
                    // Get the json data for the whole spreadsheet file
                    string jsonString = reader.ReadToEnd();

                    // Deserialize the json string into a Dictionary
                    spreadsheetData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
                }
                catch (JsonReaderException)
                {}
            }

            // Iterate through the deserialized JsonString data and 
            // set the data appropriately within the representation of the
            // spreadsheet within the form.
            foreach (string cell in spreadsheetData.Keys)
            {
                if(cell.Equals("version"))
                {
                    ss.SetVersion(spreadsheetData[cell]);
                }
                else
                {
                    ss.SetContentsOfCell(cell, spreadsheetData[cell]);
                }
            }
        }

        /// <summary>
        /// Receive the updates to the state from the server and process updates.
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveSpreadsheetUpdates(SocketState state)
        {
            state.callMe = ReceiveSpreadsheetUpdates;
            StaticNetworking.GetData(state);

            // Retrieve string message from state
            //string[] s = state.SB.ToString().Split('\n');
            //Byte[] b = state.MessageBuffer;

            // Lock the spreadsheet while data processes
            // Process data

            // Clear the stringbuilder for the next round of messages from the server
        }

        /// <summary>
        /// Recurring action for drawing new data from the server.
        /// </summary>
        public void ProcessUpdates(string[] spreadsheet, Byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            StreamReader reader = new StreamReader(stream);

            // Begin reading the data stream
            using (reader)
            {
                try
                {
                    string line;
                    while (reader.Peek() >= 0)
                    {
                        line = reader.ReadLine();
                    }

                } catch (Exception e) {

                }
            }
        }

        private void InputEnterButton_Click(object sender, EventArgs e)
        {
            // Send the filename to the server that the user requests
            if (NetworkInputLabel.Text.Length < 1)
            {
                StaticNetworking.Send(spreadsheetState.Socket, NetworkInputLabel.Text + "\n");
            }
        }
    }
}
