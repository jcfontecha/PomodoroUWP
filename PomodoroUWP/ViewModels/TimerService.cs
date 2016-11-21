using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PomodoroUWP.ViewModels
{
    public class TimerService
    {
        private Timer timer;

        private int timeout = 1500;
        private int count = 0;

        public int Timeout
        {
            get
            {
                return timeout; 
            }
            set
            {
                StopTimer();
                timeout = value;
                count = timeout;
            }
        }
        
        public int Interval { get; private set; } = 1000;

        public Action<int> IntervalHandler { get; set; }
        public Action FinishedHandler { get; set; }

        public bool IsRunning { get; set; } = false;

        public TimerService(int seconds)
        {
            Timeout = seconds;
            Interval = 1000;
        }

        public void StartTimer()
        {
            if (count <= 0)
            {
                throw new Exception("Due Time must be greater than cero.");
            }

            TimerCallback callback = new TimerCallback(UpdateElapsedTime);
            timer = new Timer(callback, null, 0, Interval);
            IsRunning = true;
        }

        public void PauseTimer()
        {
            timer?.Dispose();
            IsRunning = false;
        }

        public void ResumeTimer()
        {
            if (count > 0)
            {
                StartTimer();
            }
            else
            {
                throw new Exception("Timer cannot resume since it isn't paused.");
            }
        }

        public void StopTimer()
        {
            timer?.Dispose();
            count = 0;
            IsRunning = false;
        }

        public string TimeString()
        {
            return TimeToDisplayString(count);
        }

        private void UpdateElapsedTime(object state)
        {
            if (count > 0)
            {
                count--;

                RunOnUIThread(() => { IntervalHandler?.Invoke(count); });
            }
            else
            {
                RunOnUIThread(() => { FinishedHandler?.Invoke(); });
            }
        }

        private async void RunOnUIThread(Action action)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                action?.Invoke();
            });
        }

        public static string TimeToDisplayString(int count)
        {
            int minutes = count / 60;
            int seconds = count - minutes * 60;

            return CeroPrecededDigit(minutes) + ":" + CeroPrecededDigit(seconds);
        }

        public static string CeroPrecededDigit(int num)
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
    }
}
