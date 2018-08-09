////////////////////////////////////////////////////////////////////////////////
//
//  File Name : GatewayViewModel.cs
//
//  Abstract  : Provides data shaping and binding for views binding to gateway data.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.ViewModels
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides data shaping and binding for views binding to gateway data.
    /// </summary>
    public class GatewayViewModel : INotifyPropertyChanged
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // INotifyPropertyChanged Implementation
        //
        
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify 
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
        // Types and Constants
        //

        public const string VersionGuidPropertyName = "VersionGuid";
        public const string SerialNumberPropertyName = "SerialNumber";
        public const string TrackerPropertyName = "Tracker";
        public static readonly Guid TestGuid = new Guid();

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public GatewayViewModel() { }

        /// <summary>
        /// Constructor that takes a version guid.
        /// </summary>
        /// <param name="guid">Version guid.  Set to TestGuid to fill with test data.</param>
        public GatewayViewModel(Guid guid)
        {
            VersionGuid = guid;

            if (guid == TestGuid)
            {
                SerialNumber = new Gateway.SerialNumber();
                SerialNumber.BatchDate = DateTime.Now;
                SerialNumber.Comment = "Test Comment";
                SerialNumber.Id = 99;
                SerialNumber.TheSerialNumber = "123456";
                SerialNumber.SerialNumberTypeId = 0;
                SerialNumber.SerialNumberType = new Gateway.SerialNumberType();
                SerialNumber.SerialNumberType.Name = "Tag";
                SerialNumber.SerialNumberTypeId = 0;
                Tracker = new Gateway.Tracker();
                Tracker.ClientId = 55;
                Tracker.Comment = "Test tracker comment";
                Tracker.Id = 88;
                Tracker.MobileId = "001C2D0000FE0007";
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Binding point for version guid.
        /// </summary>
        public Guid VersionGuid
        {
            get
            {
                return _versionGuid;
            }
            set
            {
                _versionGuid = value;
                NotifyPropertyChanged(VersionGuidPropertyName);
            }
        }
        private Guid _versionGuid;

        /// <summary>
        ///  Binding point for serial number.
        /// </summary>
        public Gateway.SerialNumber SerialNumber
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
        private Gateway.SerialNumber _serialNumber;

        /// <summary>
        /// Binding point for tracker (asset) information.
        /// </summary>
        public Gateway.Tracker Tracker
        {
            get
            {
                return _tracker;
            }
            set
            {
                _tracker = value;
                NotifyPropertyChanged(TrackerPropertyName);
            }
        }
        private Gateway.Tracker _tracker;

    }
}
