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

        public List<JsonMessage> Messages { get; set; }

        public JsonUser()
        {
            Messages = new List<JsonMessage>();
            Date = DateTime.Now.ToString("g");
        }

        public JsonUser(UserGame user)
        {
            Id = user.User.Id;
            Login = user.User.Login;
            Image = user.User.MainPhoto.SRC;
            
            Messages = new List<JsonMessage>();
            Date = DateTime.Now.ToString("g");
        }
    }
}