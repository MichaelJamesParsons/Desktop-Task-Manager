using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using TimeTracker.Annotations;
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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Timer = GetTime();
        }

        public void SetActiveTask(Task task)
        {
            var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.StartTask(task));
            asyncTask.ContinueWith((t) =>
            {
                OnActiveTaskStartComplete(t, task);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnActiveTaskStartComplete(Task<TimeEntry> asyncTask, Task task)
        {
            ActiveTask = task;
            _activeTimeEntry = asyncTask.Result;
            IsFooterTrayVisible = true;
        }

        public void SetActiveTask(Task task, Action<System.Threading.Tasks.Task> callback)
        {
            /*if (ActiveTask != null)
            {
                EndActiveTask(callback);
            }*/
        }

        public void EndActiveTask(Action<System.Threading.Tasks.Task> callback)
        {
            var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.StartTask(ActiveTask));

            asyncTask.ContinueWith((t) =>
            {
                OnEndActiveTaskComplete(t, callback);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void OnEndActiveTaskComplete(Task<TimeEntry> asyncTask, Action<System.Threading.Tasks.Task> callback)
        {
            callback(asyncTask);
        }

        public string GetTime()
        {
            if (_activeTask == null || _activeTimeEntry == null)
            {
                return "";
            }

            var totalSeconds = _activeTask.CalculateSpan().TotalSeconds + (DateTime.Now - _activeTimeEntry.StartTime).TotalSeconds;
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

            if (time.Minutes > 0)
            {
                str += time.Seconds + " sec";
            }

            return str;
        }
    }
}
