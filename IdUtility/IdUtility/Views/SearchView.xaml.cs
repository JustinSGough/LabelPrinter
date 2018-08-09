////////////////////////////////////////////////////////////////////////////////
//
//  File Name : SearchView.xaml.cs
//
//  Abstract  : Code behind for serial number search view.
//
//  Revisions : 2015-05-01  JSG Created
//                  
///////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    
    /// <summary>
    /// Interaction logic for SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //

        public string SearchText 
        { 
            get
            {
                return serialNumber.Text;
            }
            
            set
            {
                serialNumber.Text = value;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Events
        //
        
        /// <summary>
        /// Set up code for event that can be propagate up through nested controls.
        /// </summary>
        public static readonly RoutedEvent SearchEvent = EventManager.RegisterRoutedEvent(
             "Search", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SearchView));
        
        /// <summary>
        /// Fired when a search should be performed.
        /// </summary>
        public event RoutedEventHandler Search
        {
            add { AddHandler(SearchEvent, value); }
            remove { RemoveHandler(SearchEvent, value); }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Public Constructors and Methods
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public SearchView()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Private
        //

        private void search_Click(object sender, RoutedEventArgs e)
        {
            RaiseSearchEvent();
        }

        private void RaiseSearchEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(SearchView.SearchEvent, this);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Trap return key and fire a Search event to subscribers.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Contains event information including the key pressed.
        /// </param>
        private void serialNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                // select all text in the field so that it will be replaced in its entirety if a bar code scan 
                // is done.  Afterwards, update the binding source (necessary if the field is filled by means
                // other than user typing.

                serialNumber.SelectAll();
                BindingExpression be = serialNumber.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
                RaiseSearchEvent();
            }
        }
    }
}
