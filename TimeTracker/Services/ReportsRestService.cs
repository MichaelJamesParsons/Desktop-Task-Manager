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

        public List<ReportableTask> GetWeeklyActivity()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ReportsEndpoint + "/week");
            request.Method = "DELETE";

            var response = (HttpWebResponse)request.GetResponse();
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return JsonConvert.DeserializeObject<List<ReportableTask>>(jsonString);
        }
    }
}
