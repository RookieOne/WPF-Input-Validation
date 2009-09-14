using System;
using System.Windows.Input;

namespace WpfApplication.Framework
{
    public class ActionCommand : ICommand
    {
        private readonly Action _action;

        public ActionCommand(Action action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}