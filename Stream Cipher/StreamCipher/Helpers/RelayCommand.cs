using System;
using System.Windows.Input;

namespace StreamCipher.Core 
{
        public class RelayCommand : ICommand
        {
            private Action methodToExecute;

            //func that defines if action should be executed right now

            private Func<bool> canExecuteMethod;
         

            public RelayCommand(Action action, Func<bool> canExecuteMethod)
            {
                this.methodToExecute = action; 
                this.canExecuteMethod = canExecuteMethod;
            }

            // if second parameter was not present then command can be executed at any time

            public RelayCommand(Action action) : this(action, null)
            {

            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return (canExecuteMethod == null) ? true : canExecuteMethod.Invoke();
            }

            public void Execute(object parameter)
            {
                methodToExecute();
            }

        }

        public class RelayCommandWithParam : ICommand
        {
            private Action<Object> methodToExecute;


            private Func<bool> canExecuteMethod;


            public RelayCommandWithParam(Action<Object> action, Func<bool> canExecuteMethod)
            {
                this.methodToExecute = action;
                this.canExecuteMethod = canExecuteMethod;
            }

            // if second parameter was not present than command can be executed at any time

            public RelayCommandWithParam(Action<Object> action) : this(action, null)
            {

            }

            public bool CanExecute(object parameter)
            {
                return (canExecuteMethod == null) ? true : canExecuteMethod.Invoke();
            }

            public void Execute(object parameter)
            {
                methodToExecute(parameter);
            }

            public event EventHandler CanExecuteChanged;

            public void RaiseCanExecuteChanged()
            {
                var handler = CanExecuteChanged;
                if (CanExecuteChanged != null)
                    handler(this, EventArgs.Empty);
            }


        }
}
