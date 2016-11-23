using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace PomodoroUWP.ViewModels.Commands
{
    class JFCommand<T> : ICommand
    {
        public Predicate<T> canExecute { get; set; }
        public Action<T> action;

        public event EventHandler CanExecuteChanged;

        public JFCommand(Predicate<T> canExecute, Action<T> action)
        {
            this.canExecute = canExecute;
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute((T)parameter);
            }
            else
            {
                throw new Exception("Can Execute Predicate not implemented");
            }
        }

        public void Execute(object parameter)
        {
            action?.Invoke((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                RunOnUIThread(() => { CanExecuteChanged(this, EventArgs.Empty); });
            }
        }

        private async void RunOnUIThread(Action action)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                action?.Invoke();
            });
        }
    }
}
