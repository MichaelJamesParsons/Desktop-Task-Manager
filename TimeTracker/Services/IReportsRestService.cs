using System.Collections.Generic;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    interface IReportsRestService
    {
        List<ReportableTask> GetWeeklyActivity();
    }
}
