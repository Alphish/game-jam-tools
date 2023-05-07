using System;
using System.Windows.Input;

namespace Alphicsh.JamTools.Common.Mvvm.Commands
{
    public class SimpleCommand : ICommand
    {
        private Action<object?> ExecutionAction { get; }

        // --------
        // Creation
        // --------

        private SimpleCommand(Action<object?> executionAction)
        {
            ExecutionAction = executionAction;
        }

        public static SimpleCommand From(Action executionAction)
        {
            return new SimpleCommand((parameter) => executionAction());
        }

        public static SimpleCommand WithParameter<TParam>(Action<TParam> executionAction)
        {
            return new SimpleCommand((object? parameter) =>
            {
                if (parameter == null)
                    throw new ArgumentNullException(nameof(parameter));

                var typedParameter = (TParam)parameter;
                executionAction(typedParameter);
            });
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
            ExecutionAction(parameter);
        }
    }
}
