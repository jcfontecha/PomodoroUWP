using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel;

namespace PomodoroUWP.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private Timer timer;
        private int seconds = 1500;

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
            int timeout = 25000;
            int interval = 1000;
            TimerCallback callback = new TimerCallback(UpdateElapsedTime);

            timer = new Timer(callback, null, timeout, interval);
            StartTimer();
        }

        public void StartTimer()
        {
            timer.Change(0, 1000);
        }

        public void UpdateElapsedTime(object state)
        {
            if (seconds > 0)
                seconds--;

            RunOnUIThread(() => { Display = TimeToDisplayString(seconds); }, null);
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

        private static async void RunOnUIThread(Action uiAction, Action callback)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                uiAction?.Invoke();
            });

            callback?.Invoke();
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
