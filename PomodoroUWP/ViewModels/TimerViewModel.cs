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

        private string _display = "Hellooooo";
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
                Display = TimeToDisplayString(time);
            };

            StartTimer();
        }

        public void StartTimer()
        {
            timerService.StartTimer();
        }

        private string TimeToDisplayString(int count)
        {
            int minutes = count / 60;
            int seconds = count - minutes * 60;

            return CeroPrecededDigit(minutes) + ":" + CeroPrecededDigit(seconds);
        }

        private string CeroPrecededDigit(int num)
        {
            if (num == 0)
            {
                return "00";
            }
            else if (num < 10)
            {
                return "0" + num.ToString();
            }
            else
            {
                return num.ToString();
            }
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
