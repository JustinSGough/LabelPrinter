////////////////////////////////////////////////////////////////////////////////
//
//  File Name : SerialNumberSearchModel.cs
//
//  Abstract  : Data for serial number searching.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;

    /// <summary>
    /// Data for serial number searching.
    /// </summary>
    public class SerialNumberSearchModel : INotifyPropertyChanged
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // INotifyPropertyChanged Implementation
        //

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

        public const string SerialNumberPropertyName = "SerialNumber";


        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }

            set
            {
                _serialNumber = value;
                NotifyPropertyChanged(SerialNumberPropertyName);
            }
        }
        private string _serialNumber = string.Empty;


        ///////////////////////////////////////////////////////////////////////
        //
        // Events
        //
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
