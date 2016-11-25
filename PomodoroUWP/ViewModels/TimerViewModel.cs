using PomodoroUWP.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PomodoroUWP.Models;

namespace PomodoroUWP.ViewModels
{
    public class TimerViewModel : INotifyPropertyChanged
    {
        private Pomodoro Pomodoro { get; set; }
        public ICommand ToggleStartCommand { get; set; }
        public ICommand CancelTimerCommand { get; set; }

        private string commandLabel = "Start";
        public string CommandLabel
        {
            get
            {
                return commandLabel;
            }
            set
            {
                commandLabel = value;
                OnPropertyChanged("CommandLabel");
            }
        }

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

        public PomodoroSession CurrentSession { get; set; }

        public bool IsInWorkMode
        {
            get
            {
                return Pomodoro.Mode == PomodoroMode.Work;
            }
        }

        public bool IsInBreakMode
        {
            get
            {
                return Pomodoro.Mode == PomodoroMode.Break;
            }
        }

        public TimerViewModel()
        {
            CurrentSession = new PomodoroSession();

            Pomodoro = new Pomodoro(10, 5);
            Display = Pomodoro.TimeString();

            Pomodoro.IntervalComplete += OnIntervalComplete;
            Pomodoro.StateChanged += OnTimerStateChanged;
            Pomodoro.ModeChanged += OnPomodoroChanged;

            ToggleStartCommand = new JFCommand(
                _ => true,
                _ => { ToggleTimer(); }
                );

            CancelTimerCommand = new JFCommand(
                _ => Pomodoro.State != TimerServiceState.Stopped,
                _ => { StopTimer(); }
                );
        }

        public void ToggleTimer()
        {
            if (Pomodoro.State != TimerServiceState.Running)
            {
                Pomodoro.StartTimer();
            }
            else
            {
                Pomodoro.PauseTimer();
            }
        }

        public void StopTimer()
        {
            Pomodoro.StopTimer();
        }

        public void OnIntervalComplete(object sender, TimerEventArgs e)
        {
            Display = e.Display;
            Progress = e.Progress;
        }

        public void OnTimerStateChanged(object sender, TimerEventArgs e)
        {
            switch (Pomodoro.State)
            {
                case TimerServiceState.Stopped:
                    CommandLabel = "Start";
                    break;
                case TimerServiceState.Running:
                    CommandLabel = "Pause";
                    break;
                case TimerServiceState.Paused:
                    CommandLabel = "Resume";
                    break;
                default:
                    break;
            }

            Display = Pomodoro.TimeString();
            Progress = Pomodoro.Progress;

            ((JFCommand)CancelTimerCommand).RaiseCanExecuteChanged();
        }

        private void OnTimerComplete(object sender, TimerEventArgs e)
        {
            // Should save the pomodoro session to some sort of 
            // data persistance
            throw new NotImplementedException();
        }


        private void OnPomodoroChanged(object sender, PomodoroEventArgs e)
        {
            // let whatever UI know that we are in different mode
            OnPropertyChanged("IsInWorkMode");
            OnPropertyChanged("IsInBreakMode");
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
