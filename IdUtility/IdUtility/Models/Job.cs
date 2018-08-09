////////////////////////////////////////////////////////////////////////////////
//
//  File Name : FileSelectionModel.cs
//
//  Abstract  : Data for a pending or completed print job.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Models
{
    using System;
    using System.Collections.Generic;
    using Logikos.Restoration.Printing;
    
    /// <summary>
    /// Data for a pending or completed print job.
    /// </summary>
    /// <remarks>
    /// Notification not needed yet.  Defer implementation.
    /// </remarks>
    public class Job
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public Job()
        {
            CommonConstruct();
        }

        /// <summary>
        /// Constructor that takes a version guid.
        /// </summary>
        /// <remarks>
        /// The version guid is only used to indicate that test data should be used.  In the future it could be used
        /// to assist with change detection.
        /// </remarks>
        /// <param name="guid">Version guid.</param>
        public Job(Guid guid)
        {
            CommonConstruct();
            JobGuid = guid;
        }

        
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Batch date of the entire job.
        /// </summary>
        public DateTime BatchDate { get; set; }
        
        /// <summary>
        /// Asset serial number.
        /// </summary>
        public string SerialNumber { get; set; }
        
        /// <summary>
        /// Asset type.
        /// </summary>
        public string AssetType { get; set; }
        
        /// <summary>
        /// Identifier of the asset.  Typically mac address.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Type of barcode to be printed.
        /// </summary>
        public BarCodeType BarCodeType { get; set; }
        
        /// <summary>
        /// Non-barcode text to be printed.
        /// </summary>
        public List<string> DescriptiveTextLines { get; set; }
        
        /// <summary>
        /// Text to be encoded as a bar code.
        /// </summary>
        public string BarCodeText { get; set; }

        /// <summary>
        /// Version guid.
        /// </summary>
        private Guid JobGuid { get; set; }
        

        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //
   
        private void CommonConstruct()
        {
            JobGuid = Guid.NewGuid();
            DescriptiveTextLines = new List<string>();
        }
    }
}
