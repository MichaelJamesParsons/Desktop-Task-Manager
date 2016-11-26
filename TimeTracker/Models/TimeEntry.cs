using System;

namespace TimeTracker.Models {
    public class TimeEntry {
        //Id of Time Entry
        public int Id { get; }

        //Start Time for Time Entry
        public DateTime StartTime { get; set; }

        //End Time for Time Entry
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Calculated Via EndTime - StartTime
        /// </summary>
        public TimeSpan TimeElapsed {
            get
            {
                return EndTime - StartTime;
            }
        }

        //Foreign Key related to which task it is connected to.
        public int TaskId { get; }

        /// <summary>
        /// Make a new time entry with id = "id" and relates to task with id "taskID"
        /// </summary>
        /// <param name="id"></param>
        public TimeEntry(int taskId) {
            this.TaskId = taskId;
        }
        public TimeEntry() { }

        /// <summary>
        /// Initializes or increments StartTime with "time"
        /// </summary>
        public void Start() {
            StartTime = new DateTime();
        }

        /// <summary>
        /// Initializes or increments EndTime with "time"
        /// </summary>
        public void End() {
            EndTime = new DateTime();
        }
    }
}
