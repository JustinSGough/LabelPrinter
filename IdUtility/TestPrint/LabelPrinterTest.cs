using Logikos.Restoration.Printing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TestPrint
{
    
    
    /// <summary>
    ///This is a test class for LabelPrinterTest and is intended
    ///to contain all LabelPrinterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LabelPrinterTest
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
        ///A test for LabelPrinter Constructor
        ///</summary>
        [TestMethod()]
        public void LabelPrinterConstructorTest()
        {
            // Run to look for exceptions.  

            BarCodeType barCodeType = BarCodeType.Code128;
            List<string> descriptiveTextLines = null; 
            string barCodeText = string.Empty; 
            LabelPrinter target = new LabelPrinter(barCodeType, descriptiveTextLines, barCodeText);
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for LabelPrinter Constructor
        ///</summary>
        [TestMethod()]
        public void LabelPrinterConstructorTest1()
        {
            // Run to look for exceptions.  
            LabelPrinter target = new LabelPrinter();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CommonConstruct
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Printing.dll")]
        public void CommonConstructTest()
        {
            // Run to look for exceptions. 
            LabelPrinter_Accessor target = new LabelPrinter_Accessor();
            target.CommonConstruct();
        }

        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest()
        {
            LabelPrinter target = new LabelPrinter(); 
            string actual;
            actual = target.GetPrintData();
            Assert.Inconclusive("GetPrintData():{0}", actual);
        }

        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest1()
        {
            LabelPrinter target = new LabelPrinter(); 
            string actual;
            string barCodeText = "654321";
            actual = target.GetPrintData(barCodeText);
            Assert.Inconclusive("GetPrintData({0}):{1}", barCodeText, actual);
        }

        /// <summary>
        ///A test for GetPrintData
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest2()
        {
            var target = new LabelPrinter();
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
        ///A test for PrintDataToString
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Printing.dll")]
        public void PrintDataToStringTest()
        {
            LabelPrinter_Accessor target = new LabelPrinter_Accessor(); 

            BarCodeType barCodeType = BarCodeType.Code128;

            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("aaaaa");
            descriptiveTextLines.Add("bbbbb");
            descriptiveTextLines.Add("ccccc");

            string barCodeText = "123456";
            
            string actual;
            
            actual = target.PrintDataToString(barCodeType, descriptiveTextLines, barCodeText);

            Assert.Inconclusive(
                "PrintDataToString({0}, {1}, {2})\n{3}",
                barCodeType, descriptiveTextLines, barCodeText, actual);
        
        }

        /// <summary>
        ///A test for BarCodeText
        ///</summary>
        [TestMethod()]
        public void BarCodeTextTest()
        {
            LabelPrinter target = new LabelPrinter(); 
            string expected = "123456";
            string actual;
            target.BarCodeText = expected;
            actual = target.BarCodeText;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BarCodeType
        ///</summary>
        [TestMethod()]
        public void BarCodeTypeTest()
        {
            LabelPrinter target = new LabelPrinter();
            BarCodeType expected = BarCodeType.Code128;
            BarCodeType actual;
            target.BarCodeType = expected;
            actual = target.BarCodeType;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DescriptiveTextLines
        ///</summary>
        [TestMethod()]
        public void DescriptiveTextLinesTest()
        {
            var lines = new List<string>();
            lines.Add("a");
            lines.Add("b");
            var target = new LabelPrinter(BarCodeType.Code128, lines, "0099282");

            List<string> expected = new List<string>();
            List<string> actual;
            target.DescriptiveTextLines = expected;
            actual = target.DescriptiveTextLines;
            Assert.AreEqual(expected, actual);
        }
    }
}
