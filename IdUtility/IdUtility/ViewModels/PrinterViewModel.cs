////////////////////////////////////////////////////////////////////////////////
//
//  File Name : PrinterViewModel.cs
//
//  Abstract  : Provides data shaping and binding for views that work with 
//            : printer selection data.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;
    using System.Drawing.Printing;

    /// <summary>
    /// Provides data shaping and binding for views that work with printer selection data.
    /// </summary>
    public class PrinterViewModel // Add INotifiyPropertyChanged as needed.
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
        /// Version number.
        /// </summary>
        /// <remarks>
        /// This property is not updated after construction.  
        /// </remarks>
        public Guid VersionGuid { get; private set; }

        /// <summary>
        /// Collection of installed printer names.
        /// </summary>
        public PrinterSettings.StringCollection InstalledPrinters
        {
            get
            {
                return PrinterSettings.InstalledPrinters;
            }
        }

        /// <summary>
        /// Number of copies to be printed. 
        /// </summary>
        public int NumberOfCopies 
        {
            get
            {
                return _numberOfCopies;
            }
            set
            {
                _numberOfCopies = value;
            }
        }
        private int _numberOfCopies;

        /// <summary>
        /// Printer selected by the user.
        /// </summary>
        public string SelectedPrinter { get; set; }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public PrinterViewModel()
        {
            CommonConstruct(Guid.NewGuid());
        }
        
        /// <summary>
        /// Construct with a version guid.
        /// </summary>
        /// <param name="versionGuid">Guid initializing the version.  Set to TestGuid to fill with test data.</param>
        public PrinterViewModel(Guid versionGuid)
        {
            CommonConstruct(versionGuid);
        }
        
        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //

        private void CommonConstruct(Guid versionGuid)
        {
            VersionGuid = versionGuid;

            var printerSettings = new PrinterSettings();

            SelectedPrinter = printerSettings.PrinterName;

            NumberOfCopies = 2;
        }
    }
}
