using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;
using NW.BL.Services;

namespace NW.PL.Models.Quest
{
    public class QuestGame
    {
        public int Id { get; set; }
        private QuestDTO quest { get; set; }
        public QuestDTO Quest => quest ?? (quest = QuestServices.Get(Id));
        public JsonQuest JsonQuest => new JsonQuest(this);

        public List<UserGame> users { get; set; }
        public List<JsonUser> JsonUsers => users.Select(x => x.JsonUser).ToList();
        public UserGame User(string Id) => users.FirstOrDefault(x => x.ConnectionId == Id);
        public UserGame UserId(int Id) => users.FirstOrDefault(x => x.Id == Id);
        public UserGame isCreator => users.FirstOrDefault(x => x.isCreator);

        public List<JsonMessage> Message => users.SelectMany(x => x.Messages).OrderBy(x => x.Date).ToList();

        private List<JsonTask> tasks;
        public List<JsonTask> Tasks => tasks ?? (tasks = Quest.pointDTO.Select(x => new JsonTask(x)).ToList());
        public JsonTask Task(int index) => Tasks.ElementAt(index);

        public QuestGame(int Id)
        {
            this.Id = Id;
            users = new List<UserGame>();
        }
    }
}