using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;
using NW.BL.Services;

namespace NW.PL.Models.Quest
{
    public class UserGame
    {
        public string ConnectionId { get; set; }
        public int Id { get; set; }

        private UserDTO user;
        public UserDTO User => user ?? (user = UserServices.Get(Id));
        public JsonUser JsonUser => new JsonUser(this);

        public double[] Position { get; set; }
        public List<JsonAnswer> Answers { get; set; }

        public bool isCreator { get; set; }
        public int IndexTask { get; set; }
        public int Lives { get; set; }

        public QuestGame Quest { get; set; }

        public DateTime? Win { get; set; }

        public UserGame(string ConnectionId, int Id)
        {
            this.ConnectionId = ConnectionId;
            this.Id = Id;
            IndexTask = 0;
            Lives = 3;
            Answers = new List<JsonAnswer>();
        }

        public UserGame setQuest(QuestGame quest)
        {
            this.Quest = quest;
            isCreator = quest.Quest.Creater == Id;
            return this;
        }
    }
}