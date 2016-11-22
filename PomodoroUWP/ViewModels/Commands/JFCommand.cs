﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PomodoroUWP.ViewModels.Commands
{
    class JFCommand<T> : ICommand
    {
        public Predicate<T> canExecute { get; set; }
        public Action<T> action;

        public event EventHandler CanExecuteChanged;

        public JFCommand(Predicate<T> canExecute, Action<T> action)
        {
            this.canExecute = canExecute;
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
            {
                return canExecute((T)parameter);
            }
            else
            {
                throw new Exception("Can Execute Predicate not implemented");
            }
        }

        public void Execute(object parameter)
        {
            action?.Invoke((T)parameter);
        }
    }
}