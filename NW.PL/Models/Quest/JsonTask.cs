using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;

namespace NW.PL.Models.Quest
{
    public class JsonTask
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Ask { get; set; }
        public string UserAnswer { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }

        public bool? isTrue { get; set; }

        public JsonTask()
        {

        }

        public JsonTask(JsonTask answer)
        {
            UserId = answer.UserId;
            Id = answer.Id;
            Ask = answer.Ask;
            Date = DateTime.Now;
            DateString = Date.ToString("dd.MM.yyyy HH:mm:ss");
            UserAnswer = answer.UserAnswer;
            isTrue = answer.isTrue;
        }

        public JsonTask(PointDTO task, int UserId = 0)
        {
            Id = task.Id;
            Ask = task.Task;
            Date = DateTime.Now;
            DateString = Date.ToString("dd.MM.yyyy HH:mm:ss");
            UserAnswer = null;

            if (UserId != 0) this.UserId = UserId;
        }
    }

    public class JsonAnswer : JsonTask
    {
        public string Answer { get; set; }
        public double[] Position { get; set; }

        public JsonAnswer()
        {

        }

        public JsonAnswer(JsonAnswer answer) : base(answer)
        {
            Answer = answer.Answer;
            Position = answer.Position;
        }

        public JsonAnswer(PointDTO task, int UserId = 0) : base(task, UserId)
        {
            Answer = task.Address;
            Position = new double[2] { task.Latitude, task.Longitude };
        }

        public JsonAnswer SetUserId(int UserId)
        {
            this.UserId = UserId;
            return this;
        }
    }
}