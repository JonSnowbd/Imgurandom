using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Imgurandom
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This is a string representation of the index out of the possible indices.
        /// For example, at index 3 and with a history size of 5, we are at "4 out of 5" (due to nil indexes)
        /// </summary>
        public string CurrentIndexAsString { get { return (Index + 1).ToString() + " out of " + URLHistory.Count.ToString(); } }

        /// <summary>
        /// Uses the index to fetch the current URL from the ObservableCollection URLHistory.
        /// This is settable so that changes will affect the URL in place, in the Collection.
        /// </summary>
        public string CurrentImageURL { get { return URLHistory[Index]; } set { URLHistory[Index] = value; Changed("CurrentImageURL"); Changed("URLHistory"); } }

        /// <summary>
        /// A Collection that holds 10 URLs, initially starts off with a guide on how to use this program.
        /// </summary>
        private ObservableCollection<string> _hist = new ObservableCollection<string>() { "https://i.imgur.com/0qsmNQT.png" };
        public ObservableCollection<string> URLHistory { get { return _hist; } set { _hist = value; Changed("URLHistory"); } }

        /// <summary>
        /// This is the Index, it points in URLHistory to where the current image url to be displayed is.
        /// </summary>
        private int _ind = 0;
        public int Index { get { return _ind; } set { _ind = value; Changed("Index"); Changed("CurrentImageURL"); Changed("CurrentIndexAsString"); } }
    }
}
