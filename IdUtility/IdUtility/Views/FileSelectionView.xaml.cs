////////////////////////////////////////////////////////////////////////////////
//
//  File Name : FileSelectionView.xaml.cs
//
//  Abstract  : Code behind for file selection view.
//
//  Revisions : 2015-05-01  JSG Created
//                  
///////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;    
    
    /// <summary>
    /// Interaction logic for FileSelectionView.xaml
    /// </summary>
    public partial class FileSelectionView : UserControl
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        /// <summary>
        /// Filename collected from the user.
        /// </summary>
        public string FileName
        {
            get
            {
                return filename.Text;
            }

            set
            {
                filename.Text = value;
            }
        }

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public FileSelectionView()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Events
        //

        public static readonly RoutedEvent LoadFileEvent = EventManager.RegisterRoutedEvent(
             "LoadFile", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSelectionView));

        public event RoutedEventHandler LoadFile
        {
            add { AddHandler(LoadFileEvent, value); }
            remove { RemoveHandler(LoadFileEvent, value); }
        }

        public static readonly RoutedEvent SendToDatabaseEvent = EventManager.RegisterRoutedEvent(
             "SendToDatabase", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileSelectionView));

        public event RoutedEventHandler SendToDatabase
        {
            add { AddHandler(SendToDatabaseEvent, value); }
            remove { RemoveHandler(SendToDatabaseEvent, value); }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //

        private void RaiseLoadFileEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FileSelectionView.LoadFileEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseSendToDatabaseEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(FileSelectionView.SendToDatabaseEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void button_LoadFile(object sender, RoutedEventArgs e)
        {
            RaiseLoadFileEvent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".txt"; 
            dlg.Filter = "All Files (*.*)|*.*";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename.Text = dlg.FileName;
                BindingExpression be = filename.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RaiseSendToDatabaseEvent();
        }

    }
}
