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
        public DateTime Date { get; set; }
        public string DateString { get; set; }

        public JsonTask()
        {

        }

        public JsonTask(PointDTO task, int UserId = 0)
        {
            Id = task.Id;
            Ask = task.Task;
            Date = DateTime.Now;
            DateString = Date.ToString("dd.MM.yyyy hh:mm:ss");

            if (UserId != 0) this.UserId = UserId;
        }
    }

    public class JsonAnswerUser : JsonTask
    {
        public string UserAnswer { get; set; }

        public bool? isTrue { get; set; }

        public JsonAnswerUser()
        {

        }

        public JsonAnswerUser(PointDTO task, int UserId = 0) : base(task, UserId)
        {

        }
    }

    public class JsonAnswer : JsonAnswerUser
    {
        public string Answer { get; set; }

        public JsonAnswer(PointDTO task, int UserId = 0) : base(task, UserId)
        {
            Answer = task.Address;
        }

        public JsonAnswer(JsonAnswer answer)
        {
            UserId = answer.UserId;
            Id = answer.Id;
            Ask = answer.Ask;
            Date = answer.Date;
            DateString = answer.DateString;
            UserAnswer = answer.UserAnswer;
            isTrue = answer.isTrue;
            Answer = answer.Answer;
        }

        public JsonAnswer SetUserId(int UserId)
        {
            this.UserId = UserId;
            return this;
        }
    }
}