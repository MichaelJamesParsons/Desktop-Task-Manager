using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TimeTracker.Commands;
using TimeTracker.Models;
using TimeTracker.Services;
using Task = TimeTracker.Models.Task;

namespace TimeTracker.ViewModels
{
    class MasterViewModel : BaseViewModel
    {
        /// <summary>
        /// The service used to perform CRUD operations on the tasks.
        /// </summary>
        private readonly ITaskService _taskService;

        /// <summary>
        /// The task which owns the active time entry.
        /// </summary>
        private Task _activeTask;
        public Task ActiveTask
        {
            get
            {
                return _activeTask;
            }

            set
            {
                _activeTask = value;

                //Flag the new task as active.
                if (_activeTask != null)
                {
                    _activeTask.IsActive = true;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The time entry used to track the timer of the active task.
        /// </summary>
        private TimeEntry _activeTimeEntry;
        public TimeEntry ActiveTimeEntry
        {
            get
            {
                return _activeTimeEntry;
            }
            set
            {
                _activeTimeEntry = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Toggles the visibility of the timer in the application's footer.
        /// </summary>
        private bool _isFooterTrayVisible;
        public bool IsFooterTrayVisible
        {
            get { return _isFooterTrayVisible; }
            set
            {
                _isFooterTrayVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The timer shown in the application's footer.
        /// </summary>
        private string _timer;
        public string Timer
        {
            get
            {
                return _timer;
            }

            set
            {
                _timer = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The command that is to be submitted in order 
        /// to stop the current task.
        /// </summary>
        public ICommand StopTaskCommand
        {
            get
            {
                return new ActionCommand(o =>
                {
                    EndActiveTask(t =>
                    {
                        //do nothing
                    });
                }, o => true);
            }
        }

        /// <summary>
        /// Initializes the master view model.
        /// </summary>
        /// <param name="taskService"></param>
        public MasterViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            IsFooterTrayVisible = false;
            InitializeTimer();
        }

        /// <summary>
        /// Starts the timer for the active time entry.
        /// </summary>
        private void InitializeTimer()
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        /// <summary>
        /// Updates the timer at the interval defined in InitializeTimer().
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            Timer = GetTime(_activeTask, _activeTimeEntry);

            if (_activeTask != null)
            {
                ActiveTask.TimeString = Timer;
            }
        }

        /// <summary>
        /// Set the current task to be timed.
        /// </summary>
        /// <param name="task"></param>
        public void SetActiveTask(Task task)
        {
            if (ActiveTask != null)
            {
                //Disable the current task before starting a new one.
                ActiveTask.IsActive = false;
                EndActiveTask(o =>
                {
                    ActivateTask(task);
                });
            }
            else
            {
                //No task is already active. Start the new task.
                ActivateTask(task);
            }
            
        }
        
        /// <summary>
        /// Sends request to service to start a timer for the given task.
        /// </summary>
        /// <param name="task"></param>
        private void ActivateTask(Task task)
        {
            var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.StartTask(task));
            asyncTask.ContinueWith((t) =>
            {
                OnActiveTaskStartComplete(t, task);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Callback for when a task is started.
        /// </summary>
        /// <param name="asyncTask"></param>
        /// <param name="task"></param>
        private void OnActiveTaskStartComplete(Task<TimeEntry> asyncTask, Task task)
        {
            task.IsActive = true;
            ActiveTask = task;
            ActiveTimeEntry = asyncTask.Result;
            Timer = GetTime(ActiveTask, ActiveTimeEntry);
            IsFooterTrayVisible = true;
        }

        /// <summary>
        /// Ends the timer for the current task.
        /// </summary>
        /// <param name="callback"></param>
        public void EndActiveTask(Action<System.Threading.Tasks.Task> callback)
        {
            try
            {
                var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.StopTask(ActiveTask));

                asyncTask.ContinueWith((t) =>
                {
                    OnEndActiveTaskComplete(t, callback);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception)
            {
                ResetActiveTask();
            }
        }

        /// <summary>
        /// Callback for when the current task's timer is stopped.
        /// </summary>
        /// <param name="asyncTask"></param>
        /// <param name="callback"></param>
        private void OnEndActiveTaskComplete(Task<TimeEntry> asyncTask, Action<System.Threading.Tasks.Task> callback)
        {
            TimeEntry t = asyncTask.Result;
            ActiveTask.TimeEntries.Add(t);

            callback(asyncTask);
            ResetActiveTask();
        }

        /// <summary>
        /// Clears the active task and time entry from the UI.
        /// </summary>
        public void ResetActiveTask()
        {
            if (ActiveTask != null)
            {
                ActiveTask.IsActive = false;
            }

            ActiveTask = null;
            ActiveTimeEntry = null;
            IsFooterTrayVisible = false;
        }

        /// <summary>
        /// Calculates the duration of the current task, then builds a string
        /// representation of the task's timer.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="activeTimeEntry"></param>
        /// <returns></returns>
        public string GetTime(Task t, TimeEntry activeTimeEntry)
        {
            if (t == null)
            {
                return "";
            }

            TimeSpan timeDifference;
            if (activeTimeEntry == null)
            {
                timeDifference = new TimeSpan();
            }
            else
            {
                timeDifference = DateTime.Now - activeTimeEntry.Start;
            }

            var span = t.CalculateSpan();
            var totalSeconds = (timeDifference + span).TotalSeconds;

            var time = TimeSpan.FromSeconds(totalSeconds);
            var str = "";

            if (time.Hours > 0)
            {
                str += time.Hours + " hrs ";
            }

            if (time.Minutes > 0)
            {
                str += time.Minutes + " min ";
            }

            if (time.Seconds > 0)
            {
                str += time.Seconds + " sec";
            }
            else
            {
                str += "0 sec";
            }

            return str;
        }
    }
}
