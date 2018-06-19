using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;

namespace NW.PL.Models.Quest
{
    public class JsonTask
    {
        public int Id { get; set; }
        public string Ask { get; set; }

        public JsonTask(PointDTO task)
        {
            Id = task.Id;
            Ask = task.Task;
        }
    }

    public class JsonAnswer : JsonTask
    {
        public JsonUser User { get; set; }
        public string Answer { get; set; }
        public string UserAnswer { get; set; }

        public JsonAnswer(PointDTO task, UserGame user, string answer) : base(task)
        {
            User = user.JsonUser;
            Answer = task.Address;
            UserAnswer = answer;
        }
    }
}