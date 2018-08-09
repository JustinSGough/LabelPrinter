////////////////////////////////////////////////////////////////////////////////
//
//  File Name : LabelPrinter
//
//  Abstract  : Get data for printing.  Acutal printing is not done through this class.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.Printing
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Get data for printing.  Acutal printing is not done through this class.
    /// </summary>
    public class LabelPrinter : ILabelPrinter
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // ILabelPrinter Members 
        //

        /// <summary>
        /// Type of barcode to print.
        /// </summary>
        public BarCodeType BarCodeType { get; set; }
        
        /// <summary>
        /// Lines of descriptive text to print.
        /// </summary>
        public List<string> DescriptiveTextLines { get; set; }
        
        /// <summary>
        /// Text to be translated into a barcode.
        /// </summary>
        public string BarCodeText { get; set; }

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        /// <returns>See ILabelPrinter.GetPrintData.</returns>
        /// <remarks>
        /// Derived classes should override this function.  Printing in this class is for debugging only.
        /// </remarks>
        public virtual string GetPrintData()
        {
            return PrintDataToString(this.BarCodeType, DescriptiveTextLines, BarCodeText);
        }

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        /// <remarks>
        /// Derived classes should override this function.  Printing in this class is for debugging only.
        /// </remarks>
        public virtual string GetPrintData(string barCodeText)
        {
            return PrintDataToString(this.BarCodeType, DescriptiveTextLines, barCodeText);
        }

        /// <summary>
        /// See ILabelPrinter.GetPrintData.
        /// </summary>
        /// <remarks>
        /// Derived classes should override this function.  Printing in this class is for debugging only.
        /// </remarks>
        public virtual string GetPrintData(BarCodeType barCodeType, List<string> descriptiveTextLines, string barCodeText)
        {
            return PrintDataToString(barCodeType, descriptiveTextLines, barCodeText);
        }


        ///////////////////////////////////////////////////////////////////////
        //
        // LabelPrinter Members
        //

        /// <summary>
        /// Construct with default properties.
        /// </summary>
        public LabelPrinter()
        {
            CommonConstruct();
        }

        /// <summary>
        /// Construct, setting up properties.  Most properties will stay the same after construction.
        /// </summary>
        /// <param name="barCodeType">Type of code to print.</param>
        /// <param name="descriptiveTextLines">Descriptive text to print on the label.</param>
        /// <param name="barCodeText">Bar code value.  This will usually be overridden when calling "Get" functions.</param>
        public LabelPrinter(BarCodeType barCodeType, List<string> descriptiveTextLines, string barCodeText)
        {
            const string tag = ClassName + ".LabelPrinter";

            descriptiveTextLines = (descriptiveTextLines == null) ? new List<string>() : descriptiveTextLines;
            barCodeText = (barCodeText == null) ? string.Empty : barCodeText;

            if (descriptiveTextLines.Count > MaxDescriptiveTextLineCount)
            {
                throw new ArgumentException(
                    tag + string.Format("descriptiveTextLines has more than {0} entries.", descriptiveTextLines.Count));
            }

            BarCodeType = barCodeType;
            DescriptiveTextLines = descriptiveTextLines;
            BarCodeText = barCodeText;
        }

        /// <summary>
        /// Set up default properties, allocate storage, etc.
        /// </summary>
        protected void CommonConstruct()
        {
            BarCodeType = BarCodeType.Code128;
            
            DescriptiveTextLines = new List<string>(3);
            DescriptiveTextLines.Add(DefaultDescriptiveHeader);
            DescriptiveTextLines.Add(DefaultWebLink);
            DescriptiveTextLines.Add(DefaultFccId);
            
            BarCodeText = "000000";
        }

        /// <summary>
        /// Default implementation of printing functionality.  For debugging purposes only.
        /// </summary>
        /// <param name="barCodeType"></param>
        /// <param name="descriptiveTextLines"></param>
        /// <param name="barCodeText"></param>
        /// <returns></returns>
        private string PrintDataToString(BarCodeType barCodeType, List<string> descriptiveTextLines, string barCodeText)
        {
            var output = new StringBuilder();

            output.AppendFormat("Type: {0}\n", barCodeType.ToString());

            for (int i = 0; i < descriptiveTextLines.Count; ++i)
            {
                output.AppendFormat("Text {0}: {1}\n", i, descriptiveTextLines[i]);
            }

            output.AppendLine(barCodeText);

            return output.ToString();
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constants and Other Private Data
        //

        private const int MaxDescriptiveTextLineCount = 3;

        private const string ClassName = "LabelPrinter";
        private const string ExitText = "Exit";

        private const string DefaultDescriptiveHeader = "Overview Tag 100";
        private const string DefaultWebLink = "http://overview.logikos.com";
        private const string DefaultFccId = "FCC ID: W7Z-ICP0";

    }
}
