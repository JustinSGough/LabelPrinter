using Logikos.Restoration.Printing.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Printing;
using System.Drawing.Printing;

namespace TestPrint
{
    
    
    /// <summary>
    ///This is a test class for RawPrinterHelperTest and is intended
    ///to contain all RawPrinterHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RawPrinterHelperTest
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
        /// A test for RawPrinterHelper Constructor
        ///</summary>
        [TestMethod()]
        public void RawPrinterHelperConstructorTest()
        {
            RawPrinterHelper target = new RawPrinterHelper();
            Assert.IsNotNull(target, "null object after construct.");
        }

  
        /// <summary>
        /// A test for SendBytesToPrinter
        ///</summary>
        [TestMethod()]
        public void SendBytesToPrinterTest()
        {
            string szPrinterName = GetCurrentPrinterName();
            
            if (!string.IsNullOrEmpty(szPrinterName))
            {
                bool expected = true; 
                bool actual = false;

                unsafe
                {
                    int number = 123456;
                    void* p = &number;
                    var intPtr = new IntPtr(p);
                    actual = RawPrinterHelper.SendBytesToPrinter(szPrinterName, intPtr, sizeof(int));
                }
                Assert.AreEqual(expected, actual, "Failed.  Win32 problem?");
            }
            else
            {
                Assert.Fail("Printer name is unavailable.  No printer installed or selected?");
            }
        }

         /// <summary>
        /// A test for SendFileToPrinter
        ///</summary>
        [TestMethod()]
        public void SendFileToPrinterTest()
        {
            string szPrinterName = GetCurrentPrinterName();

            if (!string.IsNullOrEmpty(szPrinterName))
            {
                string szFileName = TestFileName();
                bool expected = true;
                bool actual;
                actual = RawPrinterHelper.SendFileToPrinter(szPrinterName, szFileName);
                Assert.AreEqual(expected, actual, "Failed.  Win32 problem?");
            }
            else
            {
                Assert.Fail("Printer name is unavailable.  No printer installed or selected?");
            }
        }

        /// <summary>
        /// A test for SendStringToPrinter
        ///</summary>
        [TestMethod()]
        public void SendStringToPrinterTest()
        {
            string szPrinterName = GetCurrentPrinterName();

            if (!string.IsNullOrEmpty(szPrinterName))
            {
                string szString =
                    "^XA\n" + 
                    "^FO50,50^ADN,36,20^FDrestoration-printer-test\n" +
                    "^FS\n" +
                    "^XZ\n";

                bool expected = true; // TODO: Initialize to an appropriate value
                bool actual;
                actual = RawPrinterHelper.SendStringToPrinter(szPrinterName, szString);
                Assert.AreEqual(expected, actual, "Failed.  Win32 problem?");
            }
            else
            {
                Assert.Fail("Printer name is unavailable.  No printer installed or selected?");
            }

        }


        ///////////////////////////////////////////////////////////////////////
        //
        // Helper code
        //

        private static string TestFileName()
        {
            const string testFileName = "SendFileToPrinterTest.bin";

            FileStream f = File.Create(testFileName);
            f.WriteByte(55);
            f.Close();

            return testFileName;
        }

        string GetCurrentPrinterName()
        {
            var printDoc = new PrintDocument();
            return printDoc.PrinterSettings.PrinterName;
        }


    }
}
