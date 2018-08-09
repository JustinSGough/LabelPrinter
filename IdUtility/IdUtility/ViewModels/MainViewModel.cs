////////////////////////////////////////////////////////////////////////////////
//
//  File Name : MainViewModel.cs
//
//  Abstract  : Provides data shaping and binding for file selection views.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;

    /// <summary>
    /// Provides data shaping and binding for the main view.
    /// </summary>
    /// <remarks>
    /// This class collects multiple view models together.  Main view code 
    /// is responsible for handing these out to its child controls.
    /// </remarks>
    public class MainViewModel
    {        
        ///////////////////////////////////////////////////////////////////////
        //
        // Types and Constants
        //        
        
        public static readonly Guid TestGuid = new Guid();

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Gateway database data suitable for binding.
        /// </summary>
        public GatewayViewModel GatewayViewModel { get; set; }
        
        /// <summary>
        /// Search data suitable for binding.
        /// </summary>
        public SearchViewModel SearchViewModel { get; set; }
        
        /// <summary>
        /// Job list (printer queue) data suitable for binding.
        /// </summary>
        public JobListViewModel JobListViewModel { get; set; }
        
        /// <summary>
        /// Printer information suitable for binding.
        /// </summary>
        public PrinterViewModel PrinterViewModel { get; set; }
        
        /// <summary>
        /// File information suitable for binding.
        /// </summary>
        public FileSelectionViewModel FileSelectionViewModel { get; set; }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Constructor that takes a version guid.
        /// </summary>
        /// <param name="versionGuid">Version guid.  Set to "TestGuid" to fill with test data.</param>
        public MainViewModel(Guid versionGuid)
        {
            CommonConstruct(versionGuid);
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public MainViewModel()
        {
            CommonConstruct(Guid.NewGuid());
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //

        void CommonConstruct(Guid versionGuid)
        {
            GatewayViewModel = new GatewayViewModel(versionGuid);
            SearchViewModel = new SearchViewModel(versionGuid);
            JobListViewModel = new JobListViewModel(versionGuid);
            PrinterViewModel = new PrinterViewModel(versionGuid);
            FileSelectionViewModel = new FileSelectionViewModel(versionGuid);
        }
    }
}
