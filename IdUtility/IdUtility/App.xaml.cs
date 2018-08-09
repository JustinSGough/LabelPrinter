////////////////////////////////////////////////////////////////////////////////
//
//  File Name : App.xaml.cs
//
//  Abstract  : Applicaion logic.  Presides over most operations.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Logikos.Restoration.IdUtility.Gateway;
    using Logikos.Restoration.IdUtility.Models;
    using Logikos.Restoration.IdUtility.ViewModels;
    using Logikos.Restoration.Printing;
    using Logikos.Restoration.Printing.Windows;
    using Local = Logikos.Restoration.IdUtility.Properties;
    
    /// <summary>
    /// Applicaion logic.  Presides over most operations.
    /// </summary>
    public partial class App : Application
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Types and constants
        //

        private const string ClassName = "App";
        private const string ExitText = "Exit";
        
        /// <summary>
        /// Application settings key for gateway URI.
        /// </summary>
        private const string GatewayServiceUriKey = "GatewayServiceUri";
        
        public App()
        {
            FirstActivation = true;
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //
        
        /// <summary>
        /// Used to get data to be sent to the printer.  Not the printer itself.
        /// </summary>
        private ILabelPrinter LabelPrinter { get; set; }

        /// <summary>
        /// Typed instance of the application's main window.  Shortcut to avoid casting.
        /// </summary>
        private IdUtility.MainWindow TypedMainWindow { get; set; }

        /// <summary>
        /// Indicates if the application main window has been activated yet.  Used for one-time UI initialization.
        /// </summary>
        private bool FirstActivation { get; set; }

        /// <summary>
        /// Provides data and events needed for binding controls.  Has stuff for the "main view" which can dole out 
        /// parts as needed for its contained controls.
        /// </summary>
        private MainViewModel ViewModel { get; set; }


        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //

        /// <summary>
        /// Handles event when main window is activated.  Used here for one-time initialzation that depends on windows, 
        /// controls to already exist.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void Window_Activated(object sender, EventArgs e)
        {
            if (FirstActivation)
            {
                FirstActivation = false;

                // Store typed reference to our main window class.  Could end up null, so code using this
                // property should beware.
                //
                TypedMainWindow = MainWindow as IdUtility.MainWindow;
                
                SubscribeForMainWindowEvents(TypedMainWindow);
                
                // Create view model container.  Use unique guid to avoid use of test data.  The guid could be used for 
                // fancy versioning, but is not.
                //
                ViewModel = new MainViewModel(Guid.NewGuid());
                
                // Bind the main window to the View Model just created.
                MainWindow.DataContext = ViewModel;

                SetUpPrinter();
            }
        }

        /// <summary>
        /// Set printing defaults.  Can be overrided when actual printing is done.
        /// </summary>
        private void SetUpPrinter()
        {
            LabelPrinter = new ZebraZplII();
            LabelPrinter.BarCodeType = BarCodeType.Code128;
            List<string> descriptiveTextLines = new List<string>();
            descriptiveTextLines.Add("Overview Tag 100");
            descriptiveTextLines.Add("http://overview.logikos.com");
            descriptiveTextLines.Add("Contains FCC ID: W7Z-ICP0");
            LabelPrinter.DescriptiveTextLines = descriptiveTextLines;
        }
        
        /// <summary>
        /// Hook up handlers for events that bubble up from the main window.
        /// </summary>
        /// <param name="mainWindow">Main window object.</param>
        private void SubscribeForMainWindowEvents(IdUtility.MainWindow mainWindow)
        {
            if (mainWindow != null)
            {
                mainWindow.AddToQueue += new RoutedEventHandler(mainWindow_AddToQueue);
                mainWindow.DeleteAll += new RoutedEventHandler(mainWindow_DeleteAll);
                mainWindow.DeleteSelected += new RoutedEventHandler(mainWindow_DeleteSelected);
                mainWindow.PrintAll += new RoutedEventHandler(mainWindow_PrintAll);
                mainWindow.PrintSelected += new RoutedEventHandler(mainWindow_PrintSelected);
                mainWindow.Search += new RoutedEventHandler(mainWindow_Search);
                mainWindow.LoadFile += new RoutedEventHandler(mainWindow_LoadFile);
                mainWindow.SendToDatabase += new RoutedEventHandler(mainWindow_SendToDatabase);
                mainWindow.PrintDirect += new RoutedEventHandler(mainWindow_PrintDirect);
            }

        }

        /// <summary>
        /// Print label for the most-recently-loaded serial/tracker.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        private void mainWindow_PrintDirect(object sender, RoutedEventArgs e)
        {
            PrintDirect();
        }

        /// <summary>
        /// Open a CSV file with serial numbers and MAC address, read it and create new entries in the database.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void mainWindow_SendToDatabase(object sender, RoutedEventArgs e)
        {
            const string tag = ClassName + ".LoadTrackersFromFile:";

            string filename = ViewModel.FileSelectionViewModel.FileSelectionModel.Filename;

            Trace.TraceInformation(tag + string.Format("filename={0}", filename));

            string message = string.Format(
                Local.Resources.LoadToDatabaseConfirmation,
                filename);
            
            MessageBoxResult confirmation = MessageBox.Show(
                message,
                Local.Resources.LoadToDatabaseConfirmationCaption, 
                MessageBoxButton.OKCancel, 
                MessageBoxImage.Question);

            if (confirmation == MessageBoxResult.Cancel)
            {
                Trace.TraceInformation(tag + "Cancelled by the user.");
                return;
            }

            try
            {
                var reader = new StreamReader(filename);
                DateTime batchDateTime = File.GetCreationTime(filename);
                Gateway.GatewayEntities gatewayEntities = GetGatewayEntities();

                TrackerLoader.LoadTrackers(reader, gatewayEntities, batchDateTime);
                reader.Close();
            }
            catch (FileNotFoundException)
            {
                Trace.TraceError(tag + "File not found.");
            }
            catch (ArgumentException)
            {
                Trace.TraceError(tag + "Empty filename.");
            }

            Trace.TraceInformation(tag + ExitText);
        }

        /// <summary>
        /// Create entity object at URI in application settings.
        /// </summary>
        /// <returns>Entities if successful, null otherwise.</returns>
        private GatewayEntities GetGatewayEntities()
        {
            const string tag = ClassName + "GetGatewayService:";

            Gateway.GatewayEntities entities = null;

            IdUtility.Properties.Settings settings = new IdUtility.Properties.Settings();

            string gatewayServiceUriString = settings.GatewayServiceUri;

            if (string.IsNullOrEmpty(gatewayServiceUriString))
            {
                string errorText = string.Format(
                    Local.Resources.GatewayUriAppSettingNotFound,
                    GatewayServiceUriKey);
                Trace.TraceError(tag + errorText);
                MessageBox.Show(errorText, Local.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);                
            }
            else
            {
                entities = new Gateway.GatewayEntities(new Uri(gatewayServiceUriString));
            }

            return entities;
        }

        /// <summary>
        /// Look for serial and tracker data by serial number.  Do automated queueing, printing if applicable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainWindow_Search(object sender, RoutedEventArgs e)
        {
            const string tag = ClassName + "mainWindow_Search:";

            Trace.TraceInformation(tag);

            bool recordsFound = Search();

            if (recordsFound)
            {
                if (TypedMainWindow.AutoPrint)
                {
                    PrintDirect();
                }

                if (TypedMainWindow.AutoQueue)
                {
                    AddToQueue();
                }
            }
        }
        
        /// <summary>
        /// Send contents of serial, tracker model data directly to the printer.
        /// </summary>
        void PrintDirect()
        {
            Gateway.Tracker tracker = ViewModel.GatewayViewModel.Tracker;
            Gateway.SerialNumber serialNumber = ViewModel.GatewayViewModel.SerialNumber;

            List<Job> jobs = new List<Job>(1);
            jobs.Add(GetJobFromEntities(serialNumber, tracker));

            PrintJobs(jobs);
        }

        /// <summary>
        /// Look for and load serial, tracker information given a serial number.
        /// </summary>
        /// <returns>True if data was found, False otherwise.</returns>
        bool Search()
        {
            const string tag = ClassName + "Search:";

            string searchSerial = ViewModel.SearchViewModel.SerialNumberSearch.SerialNumber;
            Trace.TraceInformation(tag + "Serial={0}", searchSerial);

            Gateway.GatewayEntities gatewayEntities = GetGatewayEntities();

            if (gatewayEntities == null) return false;

            var queryResult =
                from
                    s in gatewayEntities.SerialNumbers
                where
                    (s.TheSerialNumber == searchSerial)
                select
                    s;

            Gateway.SerialNumber serialFound = null;
            Gateway.Tracker trackerFound = null;

            if (queryResult != null)
            {
                try
                {
                    serialFound = queryResult.First();
                }
                catch (InvalidOperationException)
                {
                    serialFound = null;
                }

                if (serialFound != null)
                {
                    var trackerQueryResult =
                        from
                            t in gatewayEntities.Trackers
                        where
                            (t.Id == serialFound.TrackerId)
                        select
                            t;

                    if (trackerQueryResult != null)
                    {
                        try
                        {
                            trackerFound = trackerQueryResult.First();
                        }
                        catch (InvalidOperationException)
                        {
                            Trace.TraceError(tag + "Tracker query returned 0 results.");
                            trackerFound = null;
                        }
                    }
                }
                else
                {
                    Trace.TraceInformation(tag + "Serial query returned 0 results.");
                }
            }
            else
            {
                Trace.TraceError(tag + "Serial query returned null.");
            }

            bool returnValue = false;

            if (serialFound == null || trackerFound == null)
            {
                MessageBox.Show(Local.Resources.NoRecordsFound, "", MessageBoxButton.OK, MessageBoxImage.Information);
                ViewModel.GatewayViewModel.SerialNumber = null;
                ViewModel.GatewayViewModel.Tracker = null;
            }
            else
            {
                ViewModel.GatewayViewModel.SerialNumber = serialFound;
                ViewModel.GatewayViewModel.Tracker = trackerFound;
                returnValue = true;
            }

            return returnValue;
        }

        /// <summary>
        /// Load a file, adding entries to the job list for possible printing later.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void mainWindow_LoadFile(object sender, RoutedEventArgs e)
        {
            string filename = ViewModel.FileSelectionViewModel.FileSelectionModel.Filename;

            List<Job> jobList = TrackerLoader.LoadTrackersFromFileToList(filename);

            if (jobList != null)
            {
                if (jobList.Count < 1)
                {
                    MessageBox.Show(
                        Local.Resources.NoRecordsFoundInFile, Local.Resources.ImportError, 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Exclamation);
                }
                else
                { 
                    foreach (Job j in jobList)
                    {
                        ViewModel.JobListViewModel.JobList.Add(j);
                    }
                }
            }
        }
        
        /// <summary>
        /// Send the selected job to the printer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainWindow_PrintSelected(object sender, RoutedEventArgs e)
        {
            try
            { 
                if (TypedMainWindow != null)
                {
                    int selectedIndex = TypedMainWindow.SelectedIndex;

                    if (selectedIndex != -1 && selectedIndex < ViewModel.JobListViewModel.JobList.Count)
                    {
                        List<Job> jobs = new List<Job>(1);
                        jobs.Add(ViewModel.JobListViewModel.JobList[selectedIndex]);

                        PrintJobs(jobs);
                    }
                } 
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Print a list of jobs.  Could be one, could be a bunch.
        /// </summary>
        /// <param name="jobs">Jobs to be printed.</param>
        private void PrintJobs(List<Job> jobs)
        {
            const string tag = ClassName + "PrintJobs:";

            try
            {
                string printerName = ViewModel.PrinterViewModel.SelectedPrinter;

                foreach (Job j in jobs)
                {
                    for (int copy = 1; copy <= ViewModel.PrinterViewModel.NumberOfCopies; ++copy)
                    {
                        string rawBytes = LabelPrinter.GetPrintData(j.BarCodeType, j.DescriptiveTextLines, j.BarCodeText);

                        RawPrinterHelper.SendStringToPrinter(printerName, rawBytes);

                        string logEntry = string.Format(
                            tag +
                            "Copy={9}, " +
                            "BatchDate={4}, BarCodeText={2}, SerialNumber={8}, AssetId={0}, AssetType={1}, " +
                            "BarCodeType={3}" +
                            "DescriptiveTextLines[0]={5}, DescriptiveTextLines[1]={6}, DescriptiveTextLines[2]={7}",
                            j.AssetId,
                            j.AssetType,
                            j.BarCodeText,
                            j.BarCodeType,
                            j.BatchDate,
                            (j.DescriptiveTextLines.Count > 0) ? j.DescriptiveTextLines[0] : string.Empty,
                            (j.DescriptiveTextLines.Count > 1) ? j.DescriptiveTextLines[1] : string.Empty,
                            (j.DescriptiveTextLines.Count > 2) ? j.DescriptiveTextLines[2] : string.Empty,
                            j.SerialNumber,
                            copy);

                        Trace.WriteLine(logEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                HandlePrintError(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void HandleGenericError(Exception e)
        {
            string errorText = string.Format(Local.Resources.GenericError, e.Message);
            Trace.TraceError(errorText);
            MessageBox.Show(errorText, Local.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        private void HandlePrintError(Exception e)
        {
            string errorText = string.Format(Local.Resources.PrintError, e.Message);
            Trace.TraceError(errorText);
            MessageBox.Show(errorText, Local.Resources.PrintErrorCaption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        /// <summary>
        /// Print everything in the job list.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void mainWindow_PrintAll(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TypedMainWindow != null)
                {
                    PrintJobs(new List<Job>(ViewModel.JobListViewModel.JobList));
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Delete selected entry in the job list.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void mainWindow_DeleteSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TypedMainWindow != null)
                {
                    int selectedIndex = TypedMainWindow.SelectedIndex;

                    if (selectedIndex != -1 && selectedIndex < ViewModel.JobListViewModel.JobList.Count)
                    {
                        ViewModel.JobListViewModel.JobList.RemoveAt(selectedIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Delete all jobs in the list.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        void mainWindow_DeleteAll(object sender, RoutedEventArgs e)
        {
            ViewModel.JobListViewModel.JobList.Clear();
        }

        /// <summary>
        /// Create a job entry from the serial, tracker information and add it to the job list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainWindow_AddToQueue(object sender, RoutedEventArgs e)
        {
            AddToQueue();
        }

        /// <summary>
        /// Create a job entry from the serial, tracker information and add it to the job list.
        /// </summary>
        private void AddToQueue()
        {
            try
            {
                if (ViewModel == null) return;

                Gateway.Tracker tracker = ViewModel.GatewayViewModel.Tracker;
                Gateway.SerialNumber serialNumber = ViewModel.GatewayViewModel.SerialNumber;

                if (tracker == null || serialNumber == null) return;

                ViewModel.JobListViewModel.JobList.Add(GetJobFromEntities(serialNumber, tracker));
            }
            catch (Exception ex)
            {
                HandleGenericError(ex);
            }
        }

        /// <summary>
        /// Create a job entry from serial number and tracker data.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="tracker"></param>
        /// <returns></returns>
        private Job GetJobFromEntities(Gateway.SerialNumber serialNumber, Gateway.Tracker tracker)
        {
            Job j = new Job();
            j.AssetId = tracker.MobileId;
            j.AssetType = (serialNumber.SerialNumberType == null) ? string.Empty : serialNumber.SerialNumberType.Name;
            j.BarCodeText = GetBarCodeText(tracker.MobileId);
            j.BarCodeType = Printing.BarCodeType.Code128;
            j.BatchDate = serialNumber.BatchDate;
            j.DescriptiveTextLines = GetDescriptiveTextLines(ViewModel.GatewayViewModel);
            j.SerialNumber = serialNumber.TheSerialNumber;

            return j;
        }

        /// <summary>
        /// Get the bar code text from the asset identifier.
        /// </summary>
        /// <param name="assetId">Asset identifier (probably mac address).</param>
        /// <returns>Bar code text.  A default value is used if no asset identifer is present.</returns>
        private string GetBarCodeText(string assetId)
        {
            string barCodeText = string.Empty;

            if (assetId != null)
            {
                if (assetId.Length >= 6)
                {
                    barCodeText = assetId.Substring(assetId.Length - 6, 6);
                }
                else
                {
                    barCodeText = assetId;
                }
            }
            else
            {
                barCodeText = "000000";
            }

            return barCodeText;
        }

        /// <summary>
        /// Get text for the non-barcode part of the label.
        /// </summary>
        /// <param name="vm">View Model from which to extract data.</param>
        /// <returns>List of lines to use for the label.</returns>
        private List<string> GetDescriptiveTextLines(GatewayViewModel vm)
        {
            var lines = new List<string>(3);

            // Future, test for type and set up "Tag" or "Node".
            // if (vm.SerialNumber.SerialNumberType...

            string topLine = string.Format(
                Local.Resources.LabelLineOneFormat, 
                Local.Resources.LabelTag, 
                Local.Resources.LabelTagVersionNumber);

            lines.Add(topLine);
            lines.Add(Local.Resources.LabelOverviewUri);
            lines.Add(Local.Resources.LabelFccId);

            return lines;
        }
    }
}
