////////////////////////////////////////////////////////////////////////////////
//
//  File Name : MainWindow.xaml.cs
//
//  Abstract  : Code behind for the Main Window
//
//  Revisions : 2015-05-01  JSG Created
//                  
///////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility
{
    using System.Windows;
    using Logikos.Restoration.IdUtility.ViewModels;    
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml.  Responsible for creating, setting up and routing events from 
    /// child controls.
    /// </summary>
    public partial class MainWindow : Window
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Public properties
        //

        /// <summary>
        /// Master view model, with data for this and child controls.
        /// </summary>
        public object ViewModel 
        {
            get
            {
                return DataContext;
            }

            set
            {
                DataContext = value;

                if (value.GetType() == typeof(MainViewModel))
                {
                    UpdateChildViewModels((MainViewModel)value);
                }
                else
                {
                    UpdateChildViewModels(null);
                }
            }
        }

        /// <summary>
        /// Index of selected job.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return spoolerView.SelectedIndex;
            }
        }

        /// <summary>
        /// Automatic load of serial, asset data option setting.
        /// </summary>
        public bool AutoLoad
        {
            get
            {
                bool? autoLoadValue = autoLoad.IsChecked;

                return (autoLoadValue != null) ? (bool)autoLoadValue : false;
            }
        }

        /// <summary>
        /// Automatic print of serial, asset data setting.
        /// </summary>
        public bool AutoPrint
        {
            get
            {
                bool? autoValue = autoPrint.IsChecked;
                return (autoValue != null) ? (bool)autoValue : false;
            }
        }

        public bool AutoQueue
        {
            get
            {
                bool? autoValue = autoQueue.IsChecked;
                return (autoValue != null) ? (bool)autoValue : false;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Public Events
        //

        // Set up event routing to allow events to propagate if this control is nested within other controls.

        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
            "Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent AddtoQueueEvent = EventManager.RegisterRoutedEvent(
            "AddToQueue", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent PrintAllEvent = EventManager.RegisterRoutedEvent(
            "PrintAll", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent PrintSelectedEvent = EventManager.RegisterRoutedEvent(
            "PrintSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent DeleteSelectedEvent = EventManager.RegisterRoutedEvent(
            "DeleteSelected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent DeleteAllEvent = EventManager.RegisterRoutedEvent(
            "DeleteAll", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent LoadFileEvent = EventManager.RegisterRoutedEvent(
            "LoadFile", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent SendToDatabaseEvent = EventManager.RegisterRoutedEvent(
             "SendToDatabase", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        public static readonly RoutedEvent PrintDirectEvent = EventManager.RegisterRoutedEvent(
             "PrintDirect", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));

        /// <summary>
        /// Signal to create new entries in the database.
        /// </summary>
        public event RoutedEventHandler SendToDatabase
        {
            add { AddHandler(SendToDatabaseEvent, value); }
            remove { RemoveHandler(SendToDatabaseEvent, value); }
        }
        
        /// <summary>
        /// Signal to print current without queueing.
        /// </summary>
        public event RoutedEventHandler PrintDirect
        {
            add { AddHandler(PrintDirectEvent, value); }
            remove { RemoveHandler(PrintDirectEvent, value); }
        }

        /// <summary>
        /// Signal to load data from a file into the queue.
        /// </summary>
        public event RoutedEventHandler LoadFile
        {
            add { AddHandler(LoadFileEvent, value); }
            remove { RemoveHandler(LoadFileEvent, value); }
        }

        /// <summary>
        /// Signal to search.
        /// </summary>
        public event RoutedEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }

        /// <summary>
        /// Signal to add an entry to the job queue.
        /// </summary>
        public event RoutedEventHandler AddToQueue
        {
            add { AddHandler(AddtoQueueEvent, value); }
            remove { RemoveHandler(AddtoQueueEvent, value); }
        }

        /// <summary>
        /// Signal to print all jobs in the queue.
        /// </summary>
        public event RoutedEventHandler PrintAll
        {
            add { AddHandler(PrintAllEvent, value); }
            remove { RemoveHandler(PrintAllEvent, value); }
        }

        /// <summary>
        /// Signal to print the selected job in the queue.
        /// </summary>
        public event RoutedEventHandler PrintSelected
        {
            add { AddHandler(PrintSelectedEvent, value); }
            remove { RemoveHandler(PrintSelectedEvent, value); }
        }

        /// <summary>
        /// Signal to delete the selected job in the queue.
        /// </summary>
        public event RoutedEventHandler DeleteSelected
        {
            add { AddHandler(DeleteSelectedEvent, value); }
            remove { RemoveHandler(DeleteSelectedEvent, value); }
        }

        /// <summary>
        /// Signal to delete all jobs in the queue.
        /// </summary>
        public event RoutedEventHandler DeleteAll
        {
            add { AddHandler(DeleteAllEvent, value); }
            remove { RemoveHandler(DeleteAllEvent, value); }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        
        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //
        
        private void addToQueue_Click(object sender, RoutedEventArgs e)
        {
            RaiseAddToQueueEvent();
        }

        private void printAll_Click(object sender, RoutedEventArgs e)
        {
            RaisePrintAllEvent();
        }

        private void printSelected_Click(object sender, RoutedEventArgs e)
        {
            RaisePrintSelectedEvent();
        }

        private void deleteSelected_Click(object sender, RoutedEventArgs e)
        {
            RaiseDeleteSelectedEvent();
        }

        private void deleteAll_Click(object sender, RoutedEventArgs e)
        {
            RaiseDeleteAllEvent();
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            RaiseSearchEvent();
        }
        
        private void searchView_Search(object sender, RoutedEventArgs e)
        {
            if (AutoLoad)
            {
                RaiseSearchEvent();
            }
        }
        
        private void loadFile_Click(object sender, RoutedEventArgs e)
        {
            RaiseLoadFileEvent();
        }

        private void sendToDatabase_Click(object sender, RoutedEventArgs e)
        {
            RaiseSendToDatabaseEvent();
        }

        private void RaisePrintDirectEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PrintDirectEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseLoadFileEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(LoadFileEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseSearchEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SearchEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseAddToQueueEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(AddtoQueueEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaisePrintAllEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PrintAllEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaisePrintSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PrintSelectedEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseDeleteSelectedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(DeleteSelectedEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseDeleteAllEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(DeleteAllEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void RaiseSendToDatabaseEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SendToDatabaseEvent, this);
            RaiseEvent(newEventArgs);
        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.GetType() == typeof(MainViewModel))
            {
                UpdateChildViewModels((MainViewModel)e.NewValue);
            }
            else
            {
                UpdateChildViewModels(null);
            }
        }

        private void UpdateChildViewModels(MainViewModel mainViewModel)
        {
            if (mainViewModel != null)
            {
                searchView.DataContext = mainViewModel.SearchViewModel;
                serialNumberView.DataContext = mainViewModel.GatewayViewModel;
                assetView.DataContext = mainViewModel.GatewayViewModel;
                spoolerView.DataContext = mainViewModel.JobListViewModel;
                printerSelectView.DataContext = mainViewModel.PrinterViewModel;
                fileSelectionView.DataContext = mainViewModel.FileSelectionViewModel;
            }
            else
            {
                searchView.DataContext = null;
                assetView.DataContext = null;
                spoolerView.DataContext = null;
                printerSelectView.DataContext = null;
                fileSelectionView.DataContext = null;
            }
        }

        private void printDirect_Click(object sender, RoutedEventArgs e)
        {
            RaisePrintDirectEvent();
        }
    }

    public class SerialModel
    {

    }
}
