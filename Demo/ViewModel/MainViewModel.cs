using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Demo.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);
    }
    public class MainViewModel : INotifyPropertyChanged
    {

        public DelegateCommand LoadCommand { get; }
        public MainViewModel()
        {
            Tree = new TreeViewModel();
            LoadCommand = new DelegateCommand(Load);
        }

        private async void Load(object? obj)
        {
            await Tree.InitTree();
        }

        private TreeViewModel _tree;

        public TreeViewModel Tree
        {
            get { return _tree; }
            set { _tree = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
