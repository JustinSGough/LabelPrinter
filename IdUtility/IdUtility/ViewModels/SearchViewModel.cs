////////////////////////////////////////////////////////////////////////////////
//
//  File Name : SearchViewModel.cs
//
//  Abstract  : Provides data shaping and binding for views that work with 
//            : asset search data.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;
    using Logikos.Restoration.IdUtility.Models;

    /// <summary>
    /// Provides data shaping and binding for views that work with 
    //  asset search data.
    /// </summary>
    public class SearchViewModel
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Types and Constants
        //
        
        /// <summary>
        /// Use as a version guid to fill object with test data.
        /// </summary>
        public static readonly Guid TestGuid = new Guid();

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Serial number search data.  
        /// </summary>
        public SerialNumberSearchModel SerialNumberSearch { get; set; }
        
        /// <summary>
        /// Version guid.
        /// </summary>
        public Guid VersionGuid { get; set; }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public SearchViewModel() 
        {
            CommonConstruct(Guid.NewGuid());
        }

        /// <summary>
        /// Constructor that takes a version guid.
        /// </summary>
        /// <param name="guid">Version guid.</param>
        /// <remarks>
        /// Guid is currently only used to signal that test data should be used.  Could be handy for versioning.
        /// </remarks>
        public SearchViewModel(Guid guid)
        {
            CommonConstruct(guid);
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //
        
        void CommonConstruct(Guid versionGuid)
        {
            VersionGuid = versionGuid;
            SerialNumberSearch = new SerialNumberSearchModel();

            if (versionGuid == TestGuid)
            {
                SerialNumberSearch.SerialNumber = "123456";
            }
        }
    }
}
