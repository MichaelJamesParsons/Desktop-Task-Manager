using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TimeTracker.Commands;
using TimeTracker.Services;
using Task = TimeTracker.Models.Task;

namespace TimeTracker.ViewModels
{
    class TaskManagerViewModel : BaseViewModel
    {
        /// <summary>
        /// A list of the tasks to be listed in the UI.
        /// </summary>
        public ObservableCollection<Task> Tasks { get; set; }

        /// <summary>
        /// A service which manages all CRUD operations for tasks.
        /// </summary>
        private readonly ITaskService _taskService;

        /// <summary>
        /// The view model of the parent window.
        /// </summary>
        private readonly MasterViewModel _masterViewModel;

        /// <summary>
        /// The task description input in the task manager's UI.
        /// </summary>
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

        /// <summary>
        /// Toggle to enable/disable the "add task" button.
        /// </summary>
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

        /// <summary>
        /// Toggle to enable/disable loading states for the task
        /// manager's UI.
        /// </summary>
        private bool _isLoadingTasks;
        public bool IsLoadingTasks
        {
            get
            {
                return _isLoadingTasks;
            }

            set
            {
                _isLoadingTasks = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Sets a new active task and begins a new time entry
        /// for the new active task.
        /// </summary>
        public ICommand StartTaskCommand
        {
            get
            {
                return new ActionCommand(o =>
                {
                    var task = (Task)o;
                    _masterViewModel.SetActiveTask(task);

                }, o => true);
            }
        }

        /// <summary>
        /// Pauses the current active task.
        /// </summary>
        public ICommand PauseTaskCommand
        {
            get
            {
                return new ActionCommand(obj =>
                {
                    _masterViewModel.EndActiveTask(t =>
                    {
                        // Do nothing.
                    });
                }, i => true);
            }
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
                    var t = System.Threading.Tasks.Task.Run(() => _taskService.Insert(new Task(TaskDescription)));

                    //Process response with callback
                    t.ContinueWith(OnAddTaskComplete, TaskScheduler.FromCurrentSynchronizationContext());
                }, x => IsAddTaskButtonEnabled);
            }
        }

        /// <summary>
        /// Deletes task from data source.
        /// </summary>
        public ICommand RemoveTaskCommand
        {
            get
            {
                return new ActionCommand(o =>
                {
                    var task = (Task)o;

                    if (task.Id != 0)
                    {
                        //Request to delete task from service
                        var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.Delete(task));

                        //Process response with callback
                        asyncTask.ContinueWith((t) =>
                        {
                            OnRemoveTaskComplete(t, task);
                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        RemoveTask(task);
                    }
                }, o => true);
            }
        }

        /// <summary>
        /// Loads a list of tasks from the data service.
        /// </summary>
        public ICommand LoadTasks
        {
            get
            {
                return new ActionCommand(o =>
                {
                    IsLoadingTasks = true;
                    //Request to delete task from service
                    var asyncTask = System.Threading.Tasks.Task.Run(() => _taskService.GetAll());

                    //Process response with callback
                    asyncTask.ContinueWith(OnTasksLoaded, TaskScheduler.FromCurrentSynchronizationContext());
                }, o => true);
            }
        }

        /// <summary>
        /// Initializes a new task manager view model.
        /// </summary>
        /// <param name="masterViewModel"></param>
        /// <param name="taskService"></param>
        public TaskManagerViewModel(MasterViewModel masterViewModel, ITaskService taskService)
        {
            _isLoadingTasks = true;
            _isAddTaskButtonEnabled = true;
            _taskService = taskService;
            _masterViewModel = masterViewModel;

            Tasks = new ObservableCollection<Task>();

            LoadTasks.Execute(null);
        }

        /// <summary>
        /// Prepends a task object created by the task service.
        /// </summary>
        /// <param name="asyncTask"></param>
        private void OnAddTaskComplete(Task<Task> asyncTask)
        {
            if (asyncTask == null)
            {
                return;
            }

            Tasks.Insert(0, asyncTask.Result);
            TaskDescription = "";
            IsAddTaskButtonEnabled = true;
        }

        /// <summary>
        /// Removes task from UI after being deleted from data source.
        /// </summary>
        /// <param name="asyncTask"></param>
        /// <param name="task"></param>
        private void OnRemoveTaskComplete(Task<bool> asyncTask, Task task)
        {
            RemoveTask(task);

            if (_masterViewModel.ActiveTask != null && task.Id == _masterViewModel.ActiveTask.Id)
            {
                _masterViewModel.ResetActiveTask();
            }
        }

        /// <summary>
        /// Removes task from UI.
        /// </summary>
        /// <param name="task"></param>
        private void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }

        /// <summary>
        /// Callback to be executed when a list of tasks have been queried from
        /// the data source.
        /// </summary>
        /// <param name="asyncTask"></param>
        private void OnTasksLoaded(Task<List<Task>> asyncTask)
        {
            var tasksToAdd = asyncTask.Result;
            Tasks.Clear();

            foreach (var t in tasksToAdd)
            {
                t.TimeString = _masterViewModel.GetTime(t, null);
                if (_masterViewModel.ActiveTask != null && _masterViewModel.ActiveTask.Id == t.Id)
                {
                    _masterViewModel.ActiveTask = t;
                }

                Tasks.Add(t);
            }

            IsLoadingTasks = false;
        }

    }
}