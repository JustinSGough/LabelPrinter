////////////////////////////////////////////////////////////////////////////////
//
//  File Name : ZebraZpill.cs
//
//  Abstract  : Implementation of zebra label printer.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.Printing
{
    using System;
    using System.Collections.Generic;

    public class ZebraZplII : LabelPrinter, ILabelPrinter
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // ILabelPrinter Members 
        //

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        /// <returns>See ILabelPrinter.GetPrintData.</returns>
        public override string GetPrintData()
        {
            return GetPrintData(BarCodeType, DescriptiveTextLines, BarCodeText);
        }

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        public override string GetPrintData(string barCodeText)
        {
            return GetPrintData(BarCodeType, DescriptiveTextLines, barCodeText);
        }

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        public override string GetPrintData(
            BarCodeType barCodeType, 
            List<string> descriptiveTextLines, 
            string barCodeText)
        {
            const string tag = ClassName + ".GetPrintData:";

            // Check arguments and fill with blanks if missing.
            //

            string zebraBarCodeDesignator;

            switch (barCodeType)
            {
                case BarCodeType.Code128:
                    zebraBarCodeDesignator = ZplBarCodeDesignatorCode128;
                    break;
                default:
                    throw new ArgumentException(
                        tag + string.Format("BarCodeType {0} not supported.", barCodeType.ToString()));
            }

            descriptiveTextLines = (descriptiveTextLines == null) ? new List<string>() : descriptiveTextLines;
            barCodeText = (barCodeText == null) ? string.Empty : barCodeText;

            // Load our 3 descriptive lines with text.
            //

            string descriptionLine1 = (descriptiveTextLines.Count < 1) ? string.Empty : descriptiveTextLines[0];
            string descriptionLine2 = (descriptiveTextLines.Count < 2) ? string.Empty : descriptiveTextLines[1];
            string descriptionLine3 = (descriptiveTextLines.Count < 3) ? string.Empty : descriptiveTextLines[2];

            // Generate output
            //

            string output = string.Format(
                PrintFormat,
                descriptionLine1,
                descriptionLine2,
                descriptionLine3,
                zebraBarCodeDesignator,
                barCodeText);

            return output;
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // ZebraZplII Members
        //

        /// <summary>
        /// Construct with default properties.
        /// </summary>
        public ZebraZplII() : base() {}

        /// <summary>
        /// Construct, setting up properties.  Most properties will stay the same after construction.
        /// </summary>
        /// <param name="barCodeType">Type of code to print.</param>
        /// <param name="descriptiveTextLines">Descriptive text to print on the label.</param>
        /// <param name="barCodeText">Bar code value.  This will usually be overridden when calling "Get" functions.</param>
        public ZebraZplII(BarCodeType barCodeType, List<string> descriptiveTextLines, string barCodeText) 
            : base(barCodeType, descriptiveTextLines, barCodeText) {}

        ///////////////////////////////////////////////////////////////////////
        //
        // Constants and Other Private Data
        //

        private const string ClassName = "ZebraZplII";
        private const string ExitText = "Exit";

        /// <summary>
        /// Format string for Zebra ZPL II printers.  See "ZPL II Programming Guide" at 
        /// http://www.zebra.com/id/zebra/na/en/documentlibrary/manuals/en/zpl_II_program_guide_en.html
        /// </summary>
        private const string PrintFormat =
            "^XA\n"         + // Start Format
            // Description Line 1
            "^FO233,20\n"   + // Field Origin: ^FOx,y where x=x-axis in dots, y=y-axis in dots
            "^APN\n"        + // Font: ^Afo,h,y where f=Zebra font code, o=orientation, h=height in dots, w=width in dots 
            "^FD{0}^FS\n"   + // Field Definition, data and Field Separator 
            // Description Line 2
            "^FO233,40\n"   + // Field Origin
            "^APN\n"        + // Font
            "^FD{1}^FS\n"   + // Field Definition, data and Field Separator 
            // Description Line 3
            "^FO233,65\n"   + // Field Origin
            "^APN\n"        + // Font
            "^FD{2}^FS\n"   + // Field Definition, data and Field Separator 
            // Bar code
            "^FO28,20\n"    + // Field Origin
            "^B{3}N,60\n"   + // Bar Code ^Bt,o,h,f,g,e,m where t=type, o=orientation, h=height in dots, f,g,e,m not used
            "^FD{4}^FS\n"      + // Field Definition, data Field Separator
            "^XZ\n";        // End Format    

        private const string ZplBarCodeDesignatorCode128 = "C";
    }
}
