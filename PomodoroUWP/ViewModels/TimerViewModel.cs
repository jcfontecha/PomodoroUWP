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
        private TimerService timerService;
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

        public TimerViewModel()
        {
            CurrentSession = new PomodoroSession();

            timerService = new TimerService(500);
            Display = timerService.TimeString();

            timerService.IntervalComplete += OnIntervalComplete;
            timerService.StateChanged += OnTimerStateChanged;
            timerService.TimerComplete += OnTimerComplete;

            ToggleStartCommand = new JFCommand<TimerViewModel>((vm) => true, (vm) => { ToggleTimer(); });
            CancelTimerCommand = new JFCommand<TimerViewModel>(
                (vm) => timerService.State != TimerServiceState.Stopped,
                (vm) => { StopTimer(); }
                );
        }

        public void ToggleTimer()
        {
            if (timerService.State != TimerServiceState.Running)
            {
                timerService.StartTimer();
            }
            else
            {
                timerService.PauseTimer();
            }
        }

        public void StopTimer()
        {
            timerService.StopTimer();
        }

        public void OnIntervalComplete(object sender, TimerEventArgs e)
        {
            Display = e.Display;
            Progress = e.Progress;
        }

        public void OnTimerStateChanged(object sender, TimerEventArgs e)
        {
            switch (timerService.State)
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

            Display = timerService.TimeString();
            Progress = timerService.Progress;

            ((JFCommand<TimerViewModel>)CancelTimerCommand).RaiseCanExecuteChanged();
        }

        private void OnTimerComplete(object sender, TimerEventArgs e)
        {
            // Should save the pomodoro session to some sort of 
            // data persistance
            throw new NotImplementedException();
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
