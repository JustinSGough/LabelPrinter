using Logikos.Restoration.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TestPrint
{
    
    
    /// <summary>
    ///This is a test class for ZebraZplIITest and is intended
    ///to contain all ZebraZplIITest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZebraZplIITest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest()
        {
            ZebraZplII target = new ZebraZplII(); 
            string expected = string.Empty; 
            string actual;
            actual = target.GetPrintData();

            Assert.Inconclusive("GetPrintData()\n{0}", actual);
        }

        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetTestToFile()
        {
            const string OutputFile = "GetTestToFile.txt";

            StreamWriter sw = new StreamWriter(OutputFile);
            sw.AutoFlush = true;

            ZebraZplII target = new ZebraZplII();
            
            target.BarCodeType = BarCodeType.Code128;

            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("Overview Tag 100");
            descriptiveTextLines.Add("http://overview.logikos.com");
            descriptiveTextLines.Add("Contains FCC ID: W7Z-ICP0");
            
            target.DescriptiveTextLines = descriptiveTextLines;

            string[] testCode = 
            {
                "FE0007",
                "123456",
                "AAAAAA",
                "000000",
                "Toooo long"
            };
            
            string actual;

            foreach (string c in testCode)
            {
                actual = target.GetPrintData(c);
                sw.Write(actual);
            }

            sw.Close();

            Assert.Inconclusive("GetPrintData() Inspect output in " + OutputFile);
        }
        
        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest1()
        {
            ZebraZplII target = new ZebraZplII();
            BarCodeType barCodeType = BarCodeType.Code128;

            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("aaaaa");
            descriptiveTextLines.Add("bbbbb");
            descriptiveTextLines.Add("ccccc");

            string barCodeText = "123456";
            string actual;

            actual = target.GetPrintData(barCodeType, descriptiveTextLines, barCodeText);


            Assert.Inconclusive(
                "GetPrintData({0}, {1}, {2})\n{3}",
                barCodeType, descriptiveTextLines, barCodeText, actual);
        }

        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest2()
        {
            ZebraZplII target = new ZebraZplII();

            BarCodeType barCodeType = BarCodeType.Code128;
            target.BarCodeType = barCodeType;

            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("aaaaa");
            descriptiveTextLines.Add("bbbbb");
            descriptiveTextLines.Add("ccccc");
            target.DescriptiveTextLines = descriptiveTextLines;

            string barCodeText = "123456";
            string actual;

            actual = target.GetPrintData(barCodeText);

            Assert.Inconclusive("GetPrintData({0})\n{1}", barCodeText, actual);
        }
        
        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest3()
        {
            ZebraZplII target = new ZebraZplII();

            BarCodeType barCodeType = BarCodeType.TestOnlyDoNotUse;
            target.BarCodeType = barCodeType;

            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("aaaaa");
            descriptiveTextLines.Add("bbbbb");
            descriptiveTextLines.Add("ccccc");
            target.DescriptiveTextLines = descriptiveTextLines;

            string barCodeText = "123456";
            string actual;

            bool exceptionThrown = false;

            try
            {
                actual = target.GetPrintData(barCodeText);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "ArgumentException was not thrown as expected for bad BarCodeType");
        }

        /// <summary>
        ///A test for ZebraZplII Constructor
        ///</summary>
        [TestMethod()]
        public void ZebraZplIIConstructorTest1()
        {
            ZebraZplII target = new ZebraZplII();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ZebraZplII Constructor
        ///</summary>
        [TestMethod()]
        public void ZebraZplIIConstructorTest2()
        {
            BarCodeType barCodeType = BarCodeType.Code128;
            
            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("1");
            descriptiveTextLines.Add("2");
            descriptiveTextLines.Add("3");

            string barCodeText = "123456";

            ZebraZplII target = new ZebraZplII(barCodeType, descriptiveTextLines, barCodeText);

            Assert.IsNotNull(target, "null from constructor");
            Assert.IsTrue(barCodeType == target.BarCodeType, "barCodeType !=");
            Assert.IsTrue(barCodeText == target.BarCodeText, "barCodeText !=");
            Assert.IsTrue(descriptiveTextLines[0] == target.DescriptiveTextLines[0], "descriptiveTextLines[0] !=");
            Assert.IsTrue(descriptiveTextLines[1] == target.DescriptiveTextLines[1], "descriptiveTextLines[1] !=");
            Assert.IsTrue(descriptiveTextLines[2] == target.DescriptiveTextLines[2], "descriptiveTextLines[2] !=");
        }

    }
}
