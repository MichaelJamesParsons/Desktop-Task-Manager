using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TimeTracker.Models;

namespace Service {
    interface ITaskRepo {
        Task Find(int id);
        List<Task> GetAll();
        List<Task> GetAllRunning();
        List<Task> GetAllStopped();
        List<TimeEntry> GetAllTimeEntries(int id);

        TimeEntry StartTask(int id);
        TimeEntry StopTask(int id);

        void Insert(int id);
        void Update(int id);
        bool Delete(int id);
    }

    abstract class TaskService : ITaskRepo {
        private static TaskService instance;
        public static TaskService Instance { get { return instance; } }

        public TaskService() {
            instance = this;
        }

        public abstract Task Find(int id);
        public abstract List<Task> GetAll();
        public abstract void Insert(int id);
        public abstract void Update(int id);
        public abstract bool Delete(int id);
        public abstract List<Task> GetAllRunning();
        public abstract List<Task> GetAllStopped();
        public abstract List<TimeEntry> GetAllTimeEntries(int id);
        public abstract TimeEntry StartTask(int id);
        public abstract TimeEntry StopTask(int id);
    }

    class RestTaskService : TaskService{
        public string taskEndpoint = @"http://138.197.15.79/task/";

        public List<Task> Tasks = new List<Task>();
        public List<TimeEntry> TimeEntries = new List<TimeEntry>();

        public override bool Delete(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id);
                request.Method = "DELETE";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                Tasks = Tasks.Where(t => t.Id != id).ToList();
                return JsonConvert.DeserializeObject<DeleteResponse>(json_string).success;
            }
            catch (Exception e) {
                throw e;
            }

        }

        public override Task Find(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id);
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                return JsonConvert.DeserializeObject<Task>(json_string);
            }
            catch (Exception e) {
                throw e;
            }
        }

        public override List<Task> GetAll() {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + "all/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                Tasks = ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e) {
                throw e;
            }
            return Tasks;
        }

        public override List<Task> GetAllRunning() {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + "running/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                Tasks = ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e) {
                throw e;
            }
            return Tasks;
        }

        public override List<Task> GetAllStopped() {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + "stopped/");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                Tasks = ((JArray) json.GetValue("tasks")).ToObject<List<Task>>();
            }
            catch (Exception e) {
                throw e;
            }
            return Tasks;
        }

        public override List<TimeEntry> GetAllTimeEntries(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id + "/timeentries");
                request.Method = "GET";

                var response = (HttpWebResponse) request.GetResponse();
                var json = JObject.Parse((new StreamReader(response.GetResponseStream())).ReadToEnd());

                TimeEntries = ((JArray) json.GetValue("time_entries")).ToObject<List<TimeEntry>>();
            }
            catch (Exception e) {
                throw e;
            }
            return TimeEntries;
        }

        public override void Insert(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id);
                request.Method = "POST";
                request.ContentType = "application/json";

                var json_to_send = JsonConvert.SerializeObject(new {
                    tasks = Tasks.Select(r => new TaskPost {
                        description = r.Description
                    })
                });

                using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                    streamWriter.Write(json_to_send);
                }

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

            }
            catch (Exception e) {
                throw e;
            }
        }

        public override TimeEntry StartTask(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id + "/start");
                request.Method = "POST";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                return JsonConvert.DeserializeObject<TimeEntry>(json_string);
            }
            catch (Exception e) {
                throw e;
            }
        }

        public override TimeEntry StopTask(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id + "/stop");
                request.Method = "POST";

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

                return JsonConvert.DeserializeObject<TimeEntry>(json_string);
            }
            catch (Exception e) {
                throw e;
            }
        }

        public override void Update(int id) {
            try {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(taskEndpoint + id);
                request.Method = "PATCH";
                request.ContentType = "application/json";

                var json_to_send = JsonConvert.SerializeObject(new {
                    tasks = Tasks.Select(r => new TaskPost {
                        description = r.Description
                    })
                });

                using (var streamWriter = new StreamWriter(request.GetRequestStream())) {
                    streamWriter.Write(json_to_send);
                }

                var response = (HttpWebResponse) request.GetResponse();
                var json_string = (new StreamReader(response.GetResponseStream())).ReadToEnd();
                var json_response = JObject.Parse(json_string);

            }
            catch (Exception e) {
                throw e;
            }
        }
    }

    public class TaskPost {
        public string description { get; set; }
        public string notes { get; set; }
    }
    public class DeleteResponse { public bool success {get;set;} }
}