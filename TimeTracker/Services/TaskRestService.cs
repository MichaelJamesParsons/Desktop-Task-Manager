using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    class TaskRestService : ITaskService
    {
        /// <summary>
        /// The endpoint to the task service.
        /// </summary>
        public string TaskEndpoint = @"http://138.197.15.79/task";

        /// <summary>
        /// Retrieve a specific task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task Find(int id)
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + id);
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return JsonConvert.DeserializeObject<Task>(jsonString);
        }


        /// <summary>
        /// Retrieve a list of all tasks.
        /// </summary>
        /// <returns></returns>
        public List<Task> GetAll()
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/all");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var json = JArray.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());
            return json.ToObject<List<Task>>();
        }

        /// <summary>
        /// Retrieve a list of all tasks with running time entries.
        /// </summary>
        /// <returns></returns>
        public List<Task> GetAllRunning()
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "running/");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
        }

        /// <summary>
        /// Retrieve a list all of tasks with stopped time entries.
        /// </summary>
        /// <returns></returns>
        public List<Task> GetAllStopped()
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "stopped/");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
        }

        /// <summary>
        /// Retrieve all of a task's time entries from the service.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public List<TimeEntry> GetAllTimeEntries(Task task)
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id + "/timeentries");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("time_entries")).ToObject<List<TimeEntry>>();
        }

        /// <summary>
        /// Creates a new task.
        /// 
        /// This will NOT start a time entry for the created task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public Task Insert(Task task)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint);
                request.Method = "POST";
                request.ContentType = "application/json";

                var jsonToSend = JsonConvert.SerializeObject(new
                {
                    description = task.Description
                });

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonToSend);
                }

                var response = (HttpWebResponse) request.GetResponse();

                // ReSharper disable once AssignNullToNotNullAttribute
                var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                return JsonConvert.DeserializeObject<Task>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Start a new time entry for a specific task.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public TimeEntry StartTask(Task task)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/" + task.Id + "/start");
                request.Method = "POST";

                var response = (HttpWebResponse) request.GetResponse();

                // ReSharper disable once AssignNullToNotNullAttribute
                var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                return JsonConvert.DeserializeObject<TimeEntry>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Stop a task's time entry.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public TimeEntry StopTask(Task task)
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/" + task.Id + "/stop");
            request.Method = "POST";

            var response = (HttpWebResponse) request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return JsonConvert.DeserializeObject<TimeEntry>(jsonString);
        }

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="task"></param>
        public void Update(Task task)
        {
            var request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id);
            request.Method = "PATCH";
            request.ContentType = "application/json";

            var jsonToSend = JsonConvert.SerializeObject(new
            {
                tasks = task
            });

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(jsonToSend);
            }

            request.GetResponse();
        }

        /// <summary>
        /// Delete a task and all of its time entries.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool Delete(Task task)
        {
            var request = (HttpWebRequest)WebRequest.Create(TaskEndpoint + "/" + task.Id);
            request.Method = "DELETE";

            var response = (HttpWebResponse)request.GetResponse();

            // ReSharper disable once AssignNullToNotNullAttribute
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return true;
        }
    }
}