using System;

namespace WinUI2MediaCollection.ViewModels
{
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private readonly Action action;
        private readonly Func<bool> canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action action)
            : this(action, null)
        {
        }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();

        public void Execute(object parameter) => action();

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}