using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Demo.ViewModel
{
    public class TreeItemViewModel : INotifyPropertyChanged
    {

        public TreeItemViewModel(string name)
        {
            Name = name;
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }


        private ObservableCollection<TreeItemViewModel> _children;

        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return _children; }
            set { _children = value; OnPropertyChanged(); }
        }

        private TreeItemViewModel _selectedItem;
        public TreeItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
