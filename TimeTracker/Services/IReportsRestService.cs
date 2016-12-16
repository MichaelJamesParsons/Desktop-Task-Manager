using TimeTracker.Models;

namespace TimeTracker.Services
{
    interface IReportsRestService
    {
        WeekReport GetWeeklyActivity();
    }
}
