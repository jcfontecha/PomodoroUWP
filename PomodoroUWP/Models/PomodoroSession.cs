using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PomodoroUWP;

namespace PomodoroUWP.Models
{
    public class PomodoroSession : INotifyPropertyChanged
    {
        private int duration = 1500; // in seconds
        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        private string title = "";
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public PomodoroSession() { }

        public PomodoroSession(Dictionary<string, string> values)
        {
            Deserialize(values);
        }

        public string Serialize()
        {
            string result = "";

            result += Title + ",";
            result += Date.ToString() + ",";
            result += Duration.ToString();

            return result;
        }

        public void Deserialize(IDictionary<string, string> values)
        {
            Title = values["Title"];
            DateTime newDate;
            DateTime.TryParse(values["Date"], out newDate);
            Date = newDate;
            Duration = int.Parse(values["Duration"]);
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
