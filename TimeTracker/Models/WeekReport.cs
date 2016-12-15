using System;
using System.Collections.Generic;

namespace TimeTracker.Models
{
    class WeekReport : Report
    {
        public WeekReport(List<ReportableTask> taskReports) : base(taskReports)
        {
            
        }

        public Dictionary<string, int> GetNumberOfTasksEachWeekDay()
        {
            var report = new Dictionary<string, int>();

            foreach (var taskReport in TaskReports)
            {
                foreach (var timeEntry in taskReport.TimeEntries)
                {
                    var dayOfWeek = timeEntry.Start.Date.DayOfWeek.ToString();
                    if (!report.ContainsKey(dayOfWeek))
                    {
                        report.Add(dayOfWeek, 0);
                    }

                    report[dayOfWeek] += 1;
                }
            }

            return report;
        }
    }
}
