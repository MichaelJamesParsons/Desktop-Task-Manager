using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    class ReportsRestService : IReportsRestService
    {
        public string ReportsEndpoint = @"http://138.197.15.79/task/analytics";

        public WeekReport GetWeeklyActivity()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ReportsEndpoint + "/week");
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            var taskReports = JsonConvert.DeserializeObject<List<ReportableTask>>(jsonString);
            return new WeekReport(taskReports);
        }
    }
}
