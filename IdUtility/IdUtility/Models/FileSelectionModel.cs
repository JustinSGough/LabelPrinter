////////////////////////////////////////////////////////////////////////////////
//
//  File Name : Data for file selection UI and processing.
//
//  Abstract  : Data for file selection UI and processing.  Suitable for control 
//            : binding.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Models
{
    using System;
    using System.ComponentModel;
    
    /// <summary>
    /// Data for file selection UI and processing.  Suitable for control binding.
    /// </summary>
    public class FileSelectionModel : INotifyPropertyChanged
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // INotifyPropertyChanged Implementation
        //
        
        /// <summary>
        /// Fire a property change event.  Needed for UI binding.
        /// </summary>
        /// <param name="info"></param>
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Types and constants
        //

        /// <summary>
        /// Identifier used for property change notifications.
        /// </summary>
        public const string FilenamePropertyName = "Filename";

        
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Name of file stored in the model.
        /// </summary>
        public string Filename
        {
            get
            {
                return _filename;
            }

            set
            {
                _filename = value;
                NotifyPropertyChanged(FilenamePropertyName);
            }
        }
        private string _filename = string.Empty;


        ///////////////////////////////////////////////////////////////////////
        //
        // Events
        //
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
