////////////////////////////////////////////////////////////////////////////////
//
//  File Name : JobListViewModel
//
//  Abstract  : Provides data shaping and binding for views that work with Job data.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;
    using Logikos.Restoration.IdUtility.Models;
    
    /// <summary>
    /// Provides data shaping and binding for views that work with Job data.
    /// </summary>
    public class JobListViewModel
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Types and Constants
        //
        
        /// <summary>
        /// Used in constructor to indicate that test data should be used.
        /// </summary>
        public static readonly Guid TestGuid = new Guid();

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //
        
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public JobListViewModel() 
        {
            VersionGuid = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor that takes a version guid.  Can be used to ask for test data.
        /// </summary>
        /// <param name="versionGuid"></param>
        public JobListViewModel(Guid versionGuid) 
        {
            if (versionGuid == TestGuid)
            {
                JobList = TestJobListModel();
            }
            else
            {
                JobList = new JobListModel();
            }
        }
        
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Version number.
        /// </summary>
        /// <remarks>
        /// This property is not updated after construction.  
        /// </remarks>
        public Guid VersionGuid { get; set; }

        /// <summary>
        /// Job data.
        /// </summary>
        public JobListModel JobList { get; set; }


        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //
        
        private static JobListModel TestJobListModel()
        {
            var jobList = new JobListModel();

            var j1 = new Job();
            j1.AssetId = "001C2C0200FE0001";
            j1.AssetType = "Tag";
            j1.BarCodeText = "FE0001";
            j1.BarCodeType = Printing.BarCodeType.Code128;
            j1.BatchDate = DateTime.Now;
            j1.DescriptiveTextLines.Add("Overview Tag 100");
            j1.DescriptiveTextLines.Add("http://overview.logikos.com");
            j1.DescriptiveTextLines.Add("Contains FCC ID: W7Z-ICP0");
            j1.SerialNumber = "111111";
            jobList.Add(j1);

            var j2 = new Job();
            j2.AssetId = "001C2C0200FE0002";
            j2.AssetType = "Tag";
            j2.BarCodeText = "FE0002";
            j2.BarCodeType = Printing.BarCodeType.Code128;
            j2.BatchDate = DateTime.Now;
            j2.DescriptiveTextLines.Add("Overview Tag 100");
            j2.DescriptiveTextLines.Add("http://overview.logikos.com");
            j2.DescriptiveTextLines.Add("Contains FCC ID: W7Z-ICP0");
            j2.SerialNumber = "222222";
            jobList.Add(j2);

            return jobList;
        }
    }
}
