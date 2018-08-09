using Logikos.Restoration.Printing.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestPrint
{
    
    
    /// <summary>
    ///This is a test class for RawPrinterHelper_DOCINFOATest and is intended
    ///to contain all RawPrinterHelper_DOCINFOATest Unit Tests
    ///</summary>
    [TestClass()]
    public class RawPrinterHelper_DOCINFOATest
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
        ///A test for DOCINFOA Constructor
        ///</summary>
        [TestMethod()]
        public void RawPrinterHelper_DOCINFOAConstructorTest()
        {
            RawPrinterHelper.DOCINFOA target = new RawPrinterHelper.DOCINFOA();
            Assert.IsNotNull(target, "Implement code to verify target");
        }
    }
}
