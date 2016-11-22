using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PomodoroUWP.ViewModels.Commands
{
    class StartTimerCommand : ICommand
    {
        public TimerService timer { get; private set; }

        public event EventHandler CanExecuteChanged;

        public StartTimerCommand(TimerService timerService)
        {
            timer = timerService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            timer.StartTimer();
        }
    }
}
