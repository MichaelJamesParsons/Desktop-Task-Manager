﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TimeTracker.Annotations;

namespace TimeTracker.Models {

    public class Task : INotifyPropertyChanged {
        //Id for task
        private int id { get; set; }

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

        public List<TimeEntry> TimeEntries { get; set; }

        public TimeSpan Duration => CalculateSpan();

        public event PropertyChangedEventHandler PropertyChanged;

        public Task(string description)
        {
            _description = description;
            TimeEntries = new List<TimeEntry>();
        }


        //R.3.1 The system shall validate the user input.
        private bool isValidTask() {
            bool rtn = true;

            rtn &= Description != "";

            return rtn;
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
