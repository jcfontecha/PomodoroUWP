using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer
{
    class TimerService
    {
        private Stopwatch stopWatch;

        public bool IsRunning
        {
            get
            {
                return stopWatch.IsRunning;
            }
        }

        public TimerService()
        {
            stopWatch = new Stopwatch();
        }

        public void StartTimer()
        {
            stopWatch.Start();
        }

        public void PauseTimer()
        {
            stopWatch.Stop();
        }

        public void StopTimer()
        {
            stopWatch.Reset();
        }
    }
}
