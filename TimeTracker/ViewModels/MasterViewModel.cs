using System.Windows.Input;
using TimeTracker.Commands;
using TimeTracker.Models;

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

        public bool IsActiveTask(Task t)
        {
            return (t.Id == ActiveTask.Id);
        }
    }
}
