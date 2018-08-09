////////////////////////////////////////////////////////////////////////////////
//
//  File Name : SpoolerView.xaml.cs
//
//  Abstract  : Code behind for spooler (job list) view.
//
//  Revisions : 2015-05-01  JSG Created
//                  
///////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Views
{
    using System.Windows.Controls;    
    
    /// <summary>
    /// Interaction logic for SpoolerView.xaml
    /// </summary>
    public partial class SpoolerView : UserControl
    {
        ///////////////////////////////////////////////////////////////////////
        //
        // Properties
        //
        
        /// <summary>
        /// Index of selected item in the list.  -1 if no selection is present.
        /// </summary>
        public int SelectedIndex 
        { 
            get
            {
                return spoolerList.SelectedIndex;
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //
        // Constructors
        //
        
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public SpoolerView()
        {
            InitializeComponent();
        }
    }
}
