using System;

namespace TimeTracker.Models {
    public class TimeEntry {
        //Id of Time Entry
        private int id;
        public int ID { get { return id; } }

        //Start Time for Time Entry
        private DateTime startTime;

        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        //End Time for Time Entry
        private DateTime endTime;

        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// Calculated Via EndTime - StartTime
        /// </summary>
        public TimeSpan TimeElapsed {
            get
            {
                return endTime - startTime;
            }
        }

        //Foreign Key related to which task it is connected to.
        private int taskID;
        public int TaskID { get { return taskID; } }

        /// <summary>
        /// Make a new time entry with id = "id" and relates to task with id "taskID"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskID"></param>
        public TimeEntry(byte taskID) {
            this.taskID = taskID;
        }
        public TimeEntry() { }

        /// <summary>
        /// Initializes or increments StartTime with "time"
        /// </summary>
        /// <param name="time"></param>
        public void StartTimeEntry(DateTime time) {
            startTime = time;
            //TimeEntryService.Instance.UpdateStartTime(id, time);
        }

        /// <summary>
        /// Initializes or increments EndTime with "time"
        /// </summary>
        /// <param name="time"></param>
        public void EndTimeEntry(DateTime time) {
            endTime = time;
            //TimeEntryService.Instance.UpdateEndTime(id, time);
        }

        public void StopTime() {
            //TimeEntryService.Instance.Stop(id);
        }
    }
}
