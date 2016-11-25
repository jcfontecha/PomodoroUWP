namespace PomodoroUWP.Models
{
    public enum PomodoroMode
    {
        Work, Break
    }

    public class PomodoroEventArgs
    {
        public PomodoroMode Mode { get; set; }

        public PomodoroEventArgs(PomodoroMode mode)
        {
            Mode = mode;
        }
    }

    public delegate void PomodoroEventHandler(object sender, PomodoroEventArgs e);

    public class Pomodoro : TimerService
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

                // raise the event!
                OnModeChanged(new PomodoroEventArgs(mode));
            }
        }

        public event PomodoroEventHandler ModeChanged;

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

            base.OnTimerComplete(e);
        }

        protected virtual void OnModeChanged(PomodoroEventArgs e)
        {
            ModeChanged?.Invoke(this, e);
        }
    }
}
