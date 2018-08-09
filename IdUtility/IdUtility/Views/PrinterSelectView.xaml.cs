////////////////////////////////////////////////////////////////////////////////
//
//  File Name : PrinterSelectionView.xaml.cs
//
//  Abstract  : Code behind for printer selection view.
//
//  Revisions : 2015-05-01  JSG Created
//                  
///////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Views
{
    using System.Windows.Controls;    
    
    /// <summary>
    /// Interaction logic for PrinterSelectView.xaml
    /// </summary>
    public partial class PrinterSelectView : UserControl
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Constructor
        //

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public PrinterSelectView()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //
        
        /// <summary>
        /// Name of the selected printer.
        /// </summary>
        public string SelectedPrinter
        {
            get
            {
                return printer.Text;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Methods
        //
        
        /// <summary>
        /// Check number of copies field.  Make sure it's a number and not too big.
        /// </summary>
        /// <param name="sender">Ignored.</param>
        /// <param name="e">Ignored.</param>
        /// <remarks>
        /// Value is capped to a small integer.  This might look weird to the user but it prevents inadvertently 
        /// sending too many print jobs.  Could be replaced with a confirmation dialog.
        /// </remarks>
        private void numberOfCopies_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value;

            bool validNumber = int.TryParse(numberOfCopies.Text, out value);

            if (!validNumber)
            {
                numberOfCopies.Text = "2";
            }
            else
            {
                if (value > 10)
                {
                    numberOfCopies.Text = "2";
                }
            }
        }
    }
}
