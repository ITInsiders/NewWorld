using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;

namespace NW.PL.Models.Quest
{
    public class JsonUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public double[] Position { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public bool isCreator { get; set; }
        public int Lives { get; set; }

        public List<JsonAnswer> Answers { get; set; }

        public JsonUser()
        {
            Answers = new List<JsonAnswer>();
            Date = DateTime.Now.ToString("g");
            isCreator = false;
            Lives = 3;
        }

        public JsonUser(UserGame user)
        {
            Id = user.User.Id;
            Login = user.User.Login;
            Image = user.User.MainPhoto?.SRC;
            isCreator = user.isCreator;
            Lives = user.Lives;
            Position = user.Position;

            Answers = new List<JsonAnswer>();
            Date = DateTime.Now.ToString("g");
        }
    }
}