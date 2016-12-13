using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using TimeTracker.Annotations;
using TimeTracker.Commands;
using TimeTracker.Models;
using TimeTracker.Services;
using Task = TimeTracker.Models.Task;

namespace TimeTracker.ViewModels
{
    class MasterViewModel : BaseViewModel
    {
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
                OnPropertyChanged();
            }
        }

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
        
        private readonly ITaskService _taskService;

        public MasterViewModel(ITaskService taskService)
        {
            _taskService = taskService;
            IsFooterTrayVisible = false;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Timer = GetTime(_activeTask, _activeTimeEntry);

            if (_activeTask != null)
            {
                ActiveTask.TimeString = Timer;
            }
        }

        public void SetActiveTask(Task task)
        {
            if (task == ActiveTask)
            {
                return;
            }

            var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.StartTask(task));
            asyncTask.ContinueWith((t) =>
            {
                OnActiveTaskStartComplete(t, task);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnActiveTaskStartComplete(Task<TimeEntry> asyncTask, Task task)
        {
            ActiveTask = task;
            ActiveTimeEntry = asyncTask.Result;
            Timer = GetTime(ActiveTask, ActiveTimeEntry);
            IsFooterTrayVisible = true;
        }

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
            catch (Exception e)
            {
                ResetActiveTask();
            }
        }

        private void OnEndActiveTaskComplete(Task<TimeEntry> asyncTask, Action<System.Threading.Tasks.Task> callback)
        {
            callback(asyncTask);
            ResetActiveTask();
        }

        public void ResetActiveTask()
        {
            ActiveTask = null;
            ActiveTimeEntry = null;
            IsFooterTrayVisible = false;
        }

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

            var totalSeconds = (timeDifference + t.CalculateSpan()).TotalSeconds;
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
    }
}
