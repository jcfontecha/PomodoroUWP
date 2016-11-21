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
        public int Timeout { get; private set; } = 2500;
        public int Interval { get; private set; } = 1000;

        private int count = 0;

        public Action<long> IntervalHandler { get; set; }
        public Action FinishedHandler { get; set; }

        public TimerService(int seconds)
        {
            // convert seconds to milliseconds
            Timeout = seconds * 1000;
            Interval = 1000;
            count = Timeout;
        }

        public void StartTimer()
        {
            StartTimer(Timeout);
        }

        public void StartTimer(int dueTime)
        {
            if (dueTime <= 0)
            {
                throw new Exception("Due Time must be greater than cero.");
            }

            TimerCallback callback = new TimerCallback(UpdateElapsedTime);
            timer = new Timer(callback, null, dueTime, Interval);
        }

        public void PauseTimer()
        {
            timer.Dispose();
        }

        public void ResumeTimer()
        {
            if (count > 0)
            {
                StartTimer(count);
            }
            else
            {
                throw new Exception("Timer cannot resume since it isn't paused.");
            }
        }

        public void StopTimer()
        {
            if (timer != null)
            {
                timer.Dispose();
                count = 0;
            }
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
    }
}
