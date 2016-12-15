using Newtonsoft.Json;

namespace TimeTracker.Models
{
    class ReportableTask : Task
    {
        [JsonProperty("time_entries.minutes")]
        public int TotalMinutes { get; set; }

        [JsonProperty("time_entries.seconds")]
        public int TotalSeconds { get; set; }

        public ReportableTask(string description) : base(description)
        {

        }
    }
}
