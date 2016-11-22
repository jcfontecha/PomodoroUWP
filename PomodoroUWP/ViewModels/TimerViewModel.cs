using PomodoroUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PomodoroUWP.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private TimerService timerService;
        public ICommand StartTimerCommand { get; set; }

        private string _display = "25:00";
        public string Display
        {
            get
            {
                return _display;
            }

            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }

        private float progress = 0.0f;
        public float Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public TimerViewModel()
        {
            timerService = new TimerService(1500);
            timerService.IntervalHandler = (time) => {
                Display = timerService.TimeString();
                Progress = timerService.Progress;
            };

            StartTimerCommand = new JFCommand<TimerViewModel>((vm) => true, (vm) => { StartTimer(); });
        }
        
        public void StartTimer()
        {
            timerService.StartTimer();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
