////////////////////////////////////////////////////////////////////////////////
//
//  File Name : BarCodeType.cs
//
//  Abstract  : Bar code types supported for label printing..
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.Printing
{
    /// <summary>
    /// Bar code types supported for label printing.
    /// </summary>
    public enum BarCodeType
    {
        /// <summary>
        /// High-density barcode symbology used for alphanumeric or numeric-only barcodes. It can encode all 
        /// 128 characters of ASCII.
        /// </summary>
        Code128,

        /// <summary>
        /// Invalid value used for test cases.  Do not use in production code!
        /// </summary>
        TestOnlyDoNotUse
    }
}
