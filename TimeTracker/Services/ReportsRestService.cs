using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    class ReportsRestService : IReportsRestService
    {
        /// <summary>
        /// The endpoint to the reports service.
        /// </summary>
        public string ReportsEndpoint = @"http://138.197.15.79/task/analytics";

        /// <summary>
        /// Retrieves a list of task reports.
        /// </summary>
        /// <returns></returns>
        public WeekReport GetWeeklyActivity()
        {
            var request = (HttpWebRequest)WebRequest.Create(ReportsEndpoint + "/week");
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
            var taskReports = JsonConvert.DeserializeObject<List<ReportableTask>>(jsonString);
            return new WeekReport(taskReports);
        }
    }
}
