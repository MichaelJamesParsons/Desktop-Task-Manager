using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Service;
using TimeTracker.Models;

namespace TimeTracker.Services
{
    class TaskRestService : ITaskService
    {
        public string TaskEndpoint = @"http://138.197.15.79/task";

        public bool Delete(Task task)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/" + task.Id);
            request.Method = "DELETE";

            var response = (HttpWebResponse) request.GetResponse();
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                
            return JsonConvert.DeserializeObject<DeleteResponse>(jsonString).success;
        }

        public Task Find(int id)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + id);
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

            return JsonConvert.DeserializeObject<Task>(jsonString);
        }

        public List<Task> GetAll()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/all");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            var json = JArray.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return json.ToObject<List<Task>>();
        }

        public List<Task> GetAllRunning()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "running/");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
        }

        public List<Task> GetAllStopped()
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "stopped/");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
        }

        public List<TimeEntry> GetAllTimeEntries(Task task)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id + "/timeentries");
            request.Method = "GET";

            var response = (HttpWebResponse) request.GetResponse();
            var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

            return ((JArray) json.GetValue("time_entries")).ToObject<List<TimeEntry>>();
        }

        public Task Insert(Task task)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint);
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
                var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var jsonResponse = JObject.Parse(jsonString);
                return JsonConvert.DeserializeObject<Task>(jsonString);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public TimeEntry StartTask(Task task)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/" + task.Id + "/start");
                request.Method = "POST";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                return JsonConvert.DeserializeObject<TimeEntry>(json_string);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TimeEntry StopTask(Task task)
        {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "/" + task.Id + "/stop");
                request.Method = "POST";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                return JsonConvert.DeserializeObject<TimeEntry>(json_string);
        }

        public void Update(Task task)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id);
            request.Method = "PATCH";
            request.ContentType = "application/json";

            var json_to_send = JsonConvert.SerializeObject(new
            {
                tasks = task
            });

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json_to_send);
            }

            var response = (HttpWebResponse) request.GetResponse();
            var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
            var jsonResponse = JObject.Parse(jsonString);
        }
    }
}