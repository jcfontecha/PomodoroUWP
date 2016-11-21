using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PomodoroUWP.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private TimerService timerService;

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

        public TimerViewModel()
        {
            timerService = new TimerService(1500);
            timerService.IntervalHandler = (time) => {
                Display = timerService.TimeString();
            };

            StartTimer();
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
