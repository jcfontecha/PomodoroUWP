using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroUWP.Models
{
    public enum PomodoroMode
    {
        Work, Break
    }

    class Pomodoro : TimerService
    {
        private PomodoroMode mode = PomodoroMode.Work;
        public PomodoroMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;

                switch (mode)
                {
                    case PomodoroMode.Work:
                        DueTime = WorkDuration;
                        break;
                    case PomodoroMode.Break:
                        DueTime = BreakDuration;
                        break;
                    default:
                        break;
                }

                ResetTimer();
            }
        }

        public int WorkDuration { get; set; } = 1500;
        public int BreakDuration { get; set; } = 300;

        public bool AutoAdvance { get; set; } = true;

        public Pomodoro(int workDuration, int breakDuration) : base(workDuration)
        {
            WorkDuration = workDuration;
            BreakDuration = breakDuration;
        }

        protected override void OnTimerComplete(TimerEventArgs e)
        {
            base.OnTimerComplete(e);

            if (AutoAdvance)
            {
                if (Mode == PomodoroMode.Work)
                {
                    Mode = PomodoroMode.Break;
                }
                else
                {
                    Mode = PomodoroMode.Work;
                }
            }
        }
    }
}
