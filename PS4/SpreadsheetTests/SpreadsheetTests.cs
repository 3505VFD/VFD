using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;



namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        Spreadsheet test = new Spreadsheet();

#if true
        /// <summary>
        /// Tests the returning list of dependent cells of SetCellContents with text
        /// </summary>
        [TestMethod]
        public void SetCellsTextTest()
        {
            test.SetContentsOfCell("A1", "This is a test");
        }


        /// <summary>
        /// Tests the null reference exception throw of SetCellContents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellsTextTestNullFail()
        {
            string[] sArr = new string[10];
            test.SetContentsOfCell("___", sArr[0]);
        }

        /// <summary>
        /// Tests the name exception throwing of Set Cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellsTextTestNameFail()
        {
            test.SetContentsOfCell("24X44", "This is a test");
        }


        /// <summary>
        /// Tests the name exception throwing of Set Cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellsTestNameFail02()
        {
            test.SetContentsOfCell("X1*11", "This is a test");
        }

        /// <summary>
        /// Tests the returning list of dependent cells of SetCellContents with formula
        /// </summary>
        [TestMethod]
        public void SetCellsFormulaTest()
        {
            test.SetContentsOfCell("A1", "=25+6/2");
            test.SetContentsOfCell("x1", "=A1+1");
            test.SetContentsOfCell("z1", "=3*A1+6");
        }

        /// <summary>
        /// Tests the returning list of dependent cells of SetCellContents with formula
        /// </summary>
        [TestMethod]
        public void SetCellsFormulaComplexTest()
        {
            test.SetContentsOfCell("A1", "=25+6/2");
            test.SetContentsOfCell("x1", "=A1+1");
            test.SetContentsOfCell("Z1", "=3*x1+6");
            test.SetContentsOfCell("B1", "=2+zed*x1");
            test.SetContentsOfCell("A1", "4*b1");
        }

        /// <summary>
        /// Tests the returning list of dependent cells of SetCellContents with formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellsFormulaCircularTest()
        {
            test.SetContentsOfCell("A1", "=25+6/2");
            test.SetContentsOfCell("x2", "=A1+1");
            test.SetContentsOfCell("Z1", "=3*x2+6");
            test.SetContentsOfCell("B1", "=2+zed*x2");
            test.SetContentsOfCell("A1", "=4*B1");
        }

        /// <summary>
        /// Tests the null reference exception throw of SetCellContents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellsTestNullFail()
        {             
            test.SetContentsOfCell("Z1", null);
        }

        /// <summary>
        /// Tests the name exception throwing of Set Cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellsFormulaTestNameFail()
        {
            test.SetContentsOfCell("", "=x*y");
        }

        /// <summary>
        /// Tests the returning list of dependent cells of SetCellContents with text
        /// </summary>
        [TestMethod]
        public void SetCellsDoubleTest()
        {
            test.SetContentsOfCell("A1", "5.0");
        }

        /// <summary>
        /// Tests the name exception throwing of Set Cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellsDoubleTestNameFail()
        {
            test.SetContentsOfCell("   ", "25");
        }

        /// <summary>
        /// Populates a spreadsheet and tests the return of a specific cell's contents
        /// </summary>
        [TestMethod]
        public void GetCellContentsTest()
        {
            test.SetContentsOfCell("A1", "5.0");
            Assert.AreEqual(5.0, test.GetCellContents("A1"));
        }

        /// <summary>
        /// Populates a spreadsheet and tests the return of a specific empty cell's contents
        /// </summary>
        [TestMethod]
        public void GetEmptyCellContentsTest()
        {
            Assert.AreEqual("", test.GetCellContents("A2"));
        }

        /// <summary>
        /// Test the exception throw of GetCellContents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTestFail()
        {
            test.GetCellContents("1234");
        }

        /// <summary>
        /// Test GetNamesOfAllNonEmptyCells method
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonEmptyCellsTest()
        {
            List<string> against = new List<string>(new string[] { "A1", "B1" });
            test.SetContentsOfCell("A1", "1");
            test.SetContentsOfCell("B1", "Woot");
            List<string> nonEmpties = new List<string>(test.GetNamesOfAllNonemptyCells());
            Assert.AreEqual(against[0], nonEmpties[0]);
            Assert.AreEqual(against[1], nonEmpties[1]);
        }
        
        [TestMethod]
        public void GetSavedVersionTest()
        {
            Spreadsheet ss = new Spreadsheet("TestFile", s => true, s => s, "Test Version");
            ss.Save("TestFile");
            ss.GetSavedVersion("TestFile");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestVersionFail()
        {
            Spreadsheet ss = new Spreadsheet("TestFile", s => true, s => s, "Test Version");
            ss.GetSavedVersion("TestFile0");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestNameFail()
        {
            Spreadsheet ss = new Spreadsheet("TestFile", s => true, s => s, "Test Version");
            ss.GetSavedVersion("TesFile");
        }

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionTestFail02()
        {
            Spreadsheet ss = new Spreadsheet("C:\\TestFile", s => true, s => s, "Test Version");
            ss.GetSavedVersion("C:\\TestFile");
        }

        [TestMethod]
        public void SaveTest()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "55");
            ss.Save("TestFile");
        }

        [TestMethod]
        public void SpreadsheetSaveAndGetCellValueTest()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "Default");
            ss.SetContentsOfCell("A1", "Date");
            ss.SetContentsOfCell("B1", "Spending");
            ss.SetContentsOfCell("C1", "Balance");
            ss.SetContentsOfCell("C2", "10");
            ss.SetContentsOfCell("C3", "10");
            ss.SetContentsOfCell("C4", "10");
            ss.SetContentsOfCell("C5", "10");
            ss.SetContentsOfCell("C6", "10");
            ss.SetContentsOfCell("C7", "10");
            ss.SetContentsOfCell("C10", "=C2+C3+C4+C5+C6+C7+C8+C9");
            ss.Save("Expenditure Tracking");
            Assert.AreEqual(60.0,ss.GetCellValue("C10"));
        } 

        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTestFail()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "55");
            ss.Save("C:\\TestFile");
        }

        /// <summary>
        /// Test should catch a UnauthorizedAccessException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void SaveTestFail02()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "55");
            ss.Save("C:\\TestFile");
        }

        [TestMethod]
        public void GetCellValueTest()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "55");
            Assert.AreEqual(55.0, ss.GetCellValue("A1"));
        }

        [TestMethod]
        public void GetCellValueTest02()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "=55*B1");
            Assert.AreEqual(0.0, ss.GetCellValue("A1"));
        }

        [TestMethod]
        public void GetCellValueTest03()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "Hi");
            Assert.AreEqual("Hi", ss.GetCellValue("A1"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValueTestFail()
        {
            Spreadsheet ss = new Spreadsheet(s => true, s => s, "Test Version");
            ss.SetContentsOfCell("A1", "Hi");
            Assert.AreEqual("Hi", ss.GetCellValue("1A1"));
        }

        [TestMethod]
        public void FileLoadConstructorTest()
        {
            Spreadsheet ss = new Spreadsheet("Expenditure Tracking", s => true, s => s.ToUpper(), "Default");
            Assert.AreEqual("Date",ss.GetCellValue("A1"));
        }
#endif

#if false
        /// <summary>
        /// Series of methods to test the isValidName helper method
        /// </summary>
        [TestMethod]
        public void TestTrueIsValidName()
        {
            Assert.IsTrue(test.isValidName("_x44"));
        }

        [TestMethod]
        public void TestTrueIsValidName02()
        {
            Assert.IsTrue(test.isValidName("A1"));
        }

        [TestMethod]
        public void TestTrueIsValidName03()
        {
            Assert.IsTrue(test.isValidName("b1"));
        }

        [TestMethod]
        public void TestFalseIsValidName()
        {
            Assert.IsFalse(test.isValidName("_(*^(&*^"));
        }

        [TestMethod]
        public void TestFalseIsValidName02()
        {
            Assert.IsFalse(test.isValidName("83"));
        }

        [TestMethod]
        public void TestFalseIsValidName03()
        {
            Assert.IsFalse(test.isValidName("*"));
        } 
#endif
    }
}