using System;
using Newtonsoft.Json;

namespace TimeTracker.Models {
    public class TimeEntry {
        //Id of Time Entry
        [JsonProperty("id")]
        public int Id { get; }

        //Start Time for Time Entry
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        //End Time for Time Entry
        [JsonProperty("end")]
        public DateTime? End { get; set; }

        /// <summary>
        /// Calculated Via EndTime - Start
        /// </summary>
        public TimeSpan TimeElapsed {
            get
            {
                if (Start == null || End == null)
                {
                    return new TimeSpan();
                }

                return (TimeSpan)(End - Start);
            }
        }

        //Foreign Key related to which task it is connected to.
        [JsonProperty("task_id")]
        public int TaskId { get; }

        /// <summary>
        /// Make a new time entry with id = "id" and relates to task with id "taskID"
        /// </summary>
        /// <param name="id"></param>
        public TimeEntry(int taskId) {
            this.TaskId = taskId;
            
        }

        public TimeEntry()
        {
        }

        /// <summary>
        /// Initializes or increments Start with "time"
        /// </summary>
        public void StartTimer() {
            Start = new DateTime();
        }

        /// <summary>
        /// Initializes or increments EndTime with "time"
        /// </summary>
        public void EndTimer() {
            End = new DateTime();
        }
    }
}
