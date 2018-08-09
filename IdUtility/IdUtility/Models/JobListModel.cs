////////////////////////////////////////////////////////////////////////////////
//
//  File Name : JobListModel.cs
//
//  Abstract  : Collection of jobs.  Parent class provides change noification, 
//            : which is handy for data binding.
//
//  Revisions : 2015-05-01  JSG Created
//                  
////////////////////////////////////////////////////////////////////////////////

namespace Logikos.Restoration.IdUtility.Models
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Collection of jobs.  Parent class provides change noification, which is
    /// handy for data binding.
    /// </summary>
    public class JobListModel : ObservableCollection<Job> {}
}
