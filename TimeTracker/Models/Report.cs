using System.Collections.Generic;

namespace TimeTracker.Models
{
    class Report
    {
        public List<ReportableTask> TaskReports { get; set; }

        public Report()
        {
            TaskReports = new List<ReportableTask>();
        }

        public Report(List<ReportableTask> taskReports)
        {
            TaskReports = taskReports;
        }
    }
}
