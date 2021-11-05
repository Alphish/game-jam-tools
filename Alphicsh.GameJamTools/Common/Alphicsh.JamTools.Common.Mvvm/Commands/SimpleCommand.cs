using System;
using System.Windows.Input;

namespace Alphicsh.JamTools.Common.Mvvm.Commands
{
    public class SimpleCommand : ICommand
    {
        private Action ExecutionAction { get; }

        // --------
        // Creation
        // --------

        public SimpleCommand(Action executionAction)
        {
            ExecutionAction = executionAction;
        }

        public static SimpleCommand From(Action executionAction)
        {
            return new SimpleCommand(executionAction);
        }

        // -----------------------
        // ICommand implementation
        // -----------------------

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            ExecutionAction();
        }
    }
}
