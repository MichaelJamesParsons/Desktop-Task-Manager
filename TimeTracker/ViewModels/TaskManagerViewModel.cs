using System;
using System.Collections.ObjectModel;
using TimeTracker.Models;

namespace TimeTracker.ViewModels
{
    class TaskManagerViewModel
    {
        public ObservableCollection<Task> Tasks { get; set; }

        public TaskManagerViewModel()
        {
            Tasks = new ObservableCollection<Task>();

            Task a = new Task("Task 1 with a long description");
            TimeEntry t = new TimeEntry();
            t.StartTime = new DateTime(2016, 11,  25, 13, 0, 0);
            t.EndTime = new DateTime(2016, 11,  25, 13, 20, 15);

            a.TimeEntries.Add(t);

            Tasks.Add(a);
            Tasks.Add(new Task("Task 2"));
            Tasks.Add(new Task("Task 3"));
            Tasks.Add(new Task("Task 4"));
            Tasks.Add(new Task("Task 5"));
        }
    }
}
