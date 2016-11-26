﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id);
                request.Method = "DELETE";

                var response = (HttpWebResponse) request.GetResponse();
                var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var jsonResponse = JObject.Parse(jsonString);
                
                return JsonConvert.DeserializeObject<DeleteResponse>(jsonString).success;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task Find(int id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + id);
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var jsonString = (new StreamReader(response.GetResponseStream())).ReadToEnd();

                return JsonConvert.DeserializeObject<Task>(jsonString);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Task> GetAll()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "all/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Task> GetAllRunning()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "running/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Task> GetAllStopped()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + "stopped/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                return ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<TimeEntry> GetAllTimeEntries(Task task)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id + "/timeentries");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                return ((JArray) json.GetValue("time_entries")).ToObject<List<TimeEntry>>();
            }
            catch (Exception e)
            {
                throw e;
            }
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
                throw e;
            }
        }

        public TimeEntry StartTask(Task task)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id + "/start");
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
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(TaskEndpoint + task.Id + "/stop");
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

        public void Update(Task task)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}