using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NW.PL.Models.Quest
{
    public class JsonMessage
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }

        public JsonMessage()
        {
            Date = DateTime.Now;
            DateString = Date.ToString("g");
        }
    }
}