using System.Threading.Tasks;

namespace TimeTracker.ViewModels
{
    class MasterViewModel
    {
        private Task ActiveTask { get; set; }

        public bool IsActiveTask(Task t)
        {
            return (t.Id == ActiveTask.Id);
        }
    }
}
