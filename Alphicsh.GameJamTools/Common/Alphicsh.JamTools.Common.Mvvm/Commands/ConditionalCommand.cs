using System;

namespace Alphicsh.JamTools.Common.Mvvm.Commands
{
    public class ConditionalCommand : IConditionalCommand
    {
        private Func<object?, bool> ExecutionCondition { get; }
        private Action<object?> ExecutionAction { get; }

        // --------
        // Creation
        // --------

        private ConditionalCommand(Func<object?, bool> executionCondition, Action<object?> executionAction)
        {
            ExecutionCondition = executionCondition;
            ExecutionAction = executionAction;
        }

        public static ConditionalCommand From(Func<bool> executionCondition, Action executionAction)
        {
            return new ConditionalCommand((parameter) => executionCondition(), (parameter) => executionAction());
        }

        public static ConditionalCommand WithParameter<TParam>(Func<TParam, bool> executionCondition, Action<TParam> executionAction)
        {
            return new ConditionalCommand((object? parameter) =>
            {
                if (parameter == null)
                    return false;

                var typedParameter = (TParam)parameter;
                return executionCondition(typedParameter);
            }, (object? parameter) =>
            {
                if (parameter == null)
                    throw new ArgumentNullException(nameof(parameter));

                var typedParameter = (TParam)parameter;
                executionAction(typedParameter);
            });
        }

        // ----------------------------------
        // IConditionalCommand implementation
        // ----------------------------------

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object? parameter)
        {
            return ExecutionCondition(parameter);
        }

        public void Execute(object? parameter)
        {
            ExecutionAction(parameter);
        }
    }
}
