using System.Collections.Generic;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    interface ITaskService
    {
        Task Find(int id);
        List<Task> GetAll();
        void Insert(Task task);
        void Update(Task task);
        bool Delete(Task task);
        List<Task> GetAllRunning();
        List<Task> GetAllStopped();
        List<TimeEntry> GetAllTimeEntries(Task task);
        TimeEntry StartTask(Task task);
        TimeEntry StopTask(Task task);
    }
}
