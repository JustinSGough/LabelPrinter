////////////////////////////////////////////////////////////////////////////////
//
//  File Name : FileSelectionViewModel.cs
//
//  Abstract  : Provides data shaping and binding for file selection views.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;
    using Logikos.Restoration.IdUtility.Models;    
    
    /// <summary>
    /// Provides data shaping and binding for file selection views.
    /// </summary>
    public class FileSelectionViewModel
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public FileSelectionViewModel()
        {
            CommonConstruct(Guid.NewGuid());
        }

        /// <summary>
        /// Constructor that takes a version guid.
        /// </summary>
        /// <param name="guid">Version guid.</param>
        /// <remarks>
        /// 
        /// </remarks>
        public FileSelectionViewModel(Guid guid)
        {
            CommonConstruct(guid);
        }

        void CommonConstruct(Guid versionGuid)
        {
            VersionGuid = versionGuid;
            FileSelectionModel = new FileSelectionModel();

            if (versionGuid == TestGuid)
            {
                FileSelectionModel.Filename = "c:\text.csv";
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //
    
        /// <summary>
        /// Guid to use in the constructor if test data is desired.
        /// </summary>
        public static readonly Guid TestGuid = new Guid();

        /// <summary>
        /// Model storing data.
        /// </summary>
        public FileSelectionModel FileSelectionModel { get; set; }
        
        /// <summary>
        /// Version guid.
        /// </summary>
        public Guid VersionGuid { get; set; }
    }
}
