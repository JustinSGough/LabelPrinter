////////////////////////////////////////////////////////////////////////////////
//
//  File Name : ILabelPrinter.cs
//
//  Abstract  : Asset label setup and printing..
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace Logikos.Restoration.Printing
{
    /// <summary>
    /// Asset label setup and printing.
    /// </summary>
    public interface ILabelPrinter
    {
        /// <summary>
        /// Type of barcode to print.
        /// </summary>
        BarCodeType BarCodeType { get; set; }

        /// <summary>
        /// Lines of descriptive text to print.
        /// </summary>        
        List<string> DescriptiveTextLines { get; set; }

        /// <summary>
        /// Text to be translated into a barcode.
        /// </summary>        
        string BarCodeText { get; set; }

        /// <summary>
        /// Get raw text suitable for submission to the printer.  Use existing property values.
        /// </summary>
        /// <returns>Printable text.</returns>
        string GetPrintData();

        /// <summary>
        /// Get raw text suitable for submission to the printer.
        /// </summary>
        /// <returns>Printable text.</returns>
        string GetPrintData(string barCodeText);

        /// <summary>
        /// Get raw text suitable for submission to the printer.  Override all property values.
        /// </summary>
        /// <returns>Printable text.</returns>
        string GetPrintData(BarCodeType barCodeType, List<string> descriptiveTextLines, string barCodeText);
    }
}
