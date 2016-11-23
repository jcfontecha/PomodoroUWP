using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PomodoroUWP.Models
{
    public enum TimerServiceState
    {
        Stopped, Running, Paused
    }

    public class TimerEventArgs : EventArgs
    {
        public float Progress { get; set; }
        public TimerServiceState State { get; set; }
        public string Display { get; set; }

        public TimerEventArgs(TimerServiceState state, float progress, string display)
        {
            State = state;
            Progress = progress;
            Display = display;
        }
    }

    public delegate void TimerEventHandler(object sender, TimerEventArgs e);

    public class TimerService
    {
        private Timer timer;

        public event TimerEventHandler StateChanged;
        public event TimerEventHandler IntervalComplete;
        public event TimerEventHandler TimerComplete;

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

        private TimerServiceState state = TimerServiceState.Stopped;
        public TimerServiceState State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                OnStateChanged(new TimerEventArgs(state, Progress, TimeString()));
            }
        }

        public float Progress
        {
            get
            {
                if (count == 0)
                {
                    return 1.0f;
                }
                else
                {
                    return 1.0f - count / (float)Timeout;
                }
            }
        }

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

            if (State != TimerServiceState.Running)
            {
                TimerCallback callback = new TimerCallback(UpdateElapsedTime);
                timer = new Timer(callback, null, 1000, Interval);
                State = TimerServiceState.Running;
            }
        }

        public void PauseTimer()
        {
            timer?.Dispose();
            State = TimerServiceState.Paused;
        }

        public void StopTimer()
        {
            timer?.Dispose();
            count = Timeout;
            State = TimerServiceState.Stopped;
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
                RunOnUIThread(() => {
                    OnIntervalComplete(new TimerEventArgs(State, Progress, TimeString()));
                });
            }
            else
            {
                RunOnUIThread(() => {
                    OnTimerComplete(new TimerEventArgs(State, Progress, TimeString()));
                });

                StopTimer();
            }
        }

        protected virtual void OnStateChanged(TimerEventArgs e)
        {
            StateChanged?.Invoke(this, e);
        }

        protected virtual void OnIntervalComplete(TimerEventArgs e)
        {
            IntervalComplete?.Invoke(this, e);
        }

        protected virtual void OnTimerComplete(TimerEventArgs e)
        {
            TimerComplete?.Invoke(this, e);
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
