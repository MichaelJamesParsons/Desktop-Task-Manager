using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TimeTracker.Annotations;
using TimeTracker.Commands;
using TimeTracker.Models;
using TimeTracker.Services;
using Task = TimeTracker.Models.Task;

namespace TimeTracker.ViewModels
{
    class TaskManagerViewModel : INotifyPropertyChanged
    {
        private string _taskDescription;
        public string TaskDescription
        {
            get { return _taskDescription; }
            set
            {
                _taskDescription = value;                
                OnPropertyChanged();
            }
        }

        private bool _isAddTaskButtonEnabled;
        public bool IsAddTaskButtonEnabled
        {
            get { return _isAddTaskButtonEnabled; }
            set
            {
                _isAddTaskButtonEnabled = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Task> Tasks { get; set; }
        public ITaskService taskService;

        public TaskManagerViewModel(ITaskService taskService)
        {
            _isAddTaskButtonEnabled = true;
            this.taskService = taskService;
            Tasks = new ObservableCollection<Task>();

            Task a = new Task("Task 1 with a long description");
            TimeEntry t = new TimeEntry();
            t.StartTime = new DateTime(2016, 11, 25, 13, 0, 0);
            t.EndTime = new DateTime(2016, 11, 25, 13, 20, 15);

            a.TimeEntries.Add(t);

            Tasks.Add(a);
            Tasks.Add(new Task("Task 2"));
            Tasks.Add(new Task("Task 3"));
            Tasks.Add(new Task("Task 4"));
            Tasks.Add(new Task("Task 5"));
        }


        /// <summary>
        /// Creates a task from the task description text box.
        /// </summary>
        public ICommand AddTaskCommand
        {
            get
            {
                return new ActionCommand(o =>
                {
                    //disable "add task" button
                    IsAddTaskButtonEnabled = false;

                    //Send new task to service
                    var t = System.Threading.Tasks.Task.Run(() => taskService.Insert(new Task(TaskDescription)));

                    //Process response with callback
                    t.ContinueWith(OnAddTaskComplete, TaskScheduler.FromCurrentSynchronizationContext());
                }, x => IsAddTaskButtonEnabled);
            }
        }

        /// <summary>
        /// Prepends a task object created by the task service.
        /// </summary>
        /// <param name="asyncTask"></param>
        private void OnAddTaskComplete(Task<Task> asyncTask)
        {
            Tasks.Insert(0, asyncTask.Result);
            TaskDescription = "";
            IsAddTaskButtonEnabled = true;
        }


        public ICommand RemoveTaskCommand
        {
            get
            {
                return new ActionCommand(OnRemoveTaskBegin, o => true);
            }
        }

        private void OnRemoveTaskBegin(object o)
        {
            var a = o;

            //Send new task to service
            var t = System.Threading.Tasks.Task.Run(() => taskService.Insert(new Task(TaskDescription)));

            //Process response with callback
            //t.ContinueWith(OnAddTaskComplete, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnRemoveTaskComplete(Task<Task> asyncTask)
        {
            
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}