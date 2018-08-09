////////////////////////////////////////////////////////////////////////////////
//
//  File Name : RawPrinterHelper
//
//  Abstract  : Provides and interface to Windows print functionality that is 
//            : not available through the .NET framework.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.Printing.Windows
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;    
    
    /// <summary>
    /// Provides and interface to Windows print functionality that is not available through the .NET framework.
    /// </summary>
    /// <remarks>
    /// This code is from a Microsoft Technical article.  See "How to send raw data to a printer by using Visual C# .NET"
    /// at http://support.microsoft.com/kb/322091.  Some project-specific modifications have been made.
    /// </remarks>
    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        /// <summary>
        /// When the function is given a printer name and an unmanaged array
        /// of bytes, the function sends those bytes to the print queue.
        /// </summary>
        /// <param name="szPrinterName">
        /// Windows printer name.  Same as that typically displayed in printer selection
        /// dialogs.
        /// </param>
        /// <param name="pBytes"></param>
        /// <param name="dwCount"></param>
        /// <returns>True on success, false on failure.</returns>
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "Overview Asset Label";  // This will be shown in Windows spooler lists.
            di.pDataType = "RAW";

            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        /// <summary>
        /// Read and send a file to the printer.
        /// </summary>
        /// <param name="szPrinterName">Windows printer name.</param>
        /// <param name="szFileName">File to be printed.</param>
        /// <returns></returns>
        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            bytes = br.ReadBytes(nLength);
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        /// <summary>
        /// Print a string.
        /// </summary>
        /// <param name="szPrinterName">Windows printer name.</param>
        /// <param name="szString">String to print.</param>
        /// <returns>Always true.</returns>
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            dwCount = szString.Length;
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}
