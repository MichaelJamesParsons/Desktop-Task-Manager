using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TimeTracker.Annotations;

namespace TimeTracker.Models {

    public class Task : INotifyPropertyChanged {
        //Id for task
        public int Id { get; set; }

        //Description of Task
        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        //Parent of task
        private int parentTaskID { get; set; }

        //Children of task
        private List<Task> subTasks { get; set; }

        [JsonProperty("time_entries")]
        public List<TimeEntry> TimeEntries { get; set; }

        public TimeSpan Duration => CalculateSpan();

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private string _timeString;
        public string TimeString
        {
            get { return _timeString; }

            set
            {
                _timeString = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Task(string description)
        {
            _description = description;
            TimeEntries = new List<TimeEntry>();
        }

        public TimeSpan CalculateSpan()
        {
            var seconds = TimeEntries.Sum(t => t.TimeElapsed.TotalSeconds);
            return TimeSpan.FromSeconds(seconds);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
