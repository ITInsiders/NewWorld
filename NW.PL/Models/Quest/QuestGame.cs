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
        public JsonQuestUsers JsonQuestUsers => new JsonQuestUsers(this);

        public List<UserGame> users { get; set; }
        public List<JsonUser> JsonUsers => users.Select(x => x.JsonUser).ToList();
        public UserGame User(string Id) => users.FirstOrDefault(x => x.ConnectionId == Id);
        public UserGame UserId(int Id) => users.FirstOrDefault(x => x.Id == Id);
        public UserGame Creator => users.FirstOrDefault(x => x.isCreator);

        public bool isGameOver => users.All(x => x.Win != null);
        public List<UserGame> Wins => users.Where(x => x.Win != null).OrderBy(x => x.Win).ToList();
        public List<JsonUser> JsonWins => Wins.Select(x => new JsonUser(x)).ToList();

        private List<JsonAnswer> tasks;
        public List<JsonAnswer> Tasks => tasks ?? (tasks = Quest.pointDTO.Select(x => new JsonAnswer(x)).ToList());
        public JsonAnswer Task(int index) => Tasks.ElementAt(index);

        public List<JsonAnswer> Answers => users.SelectMany(x => x.Answers).ToList();

        public QuestGame(int Id)
        {
            this.Id = Id;
            users = new List<UserGame>();
        }
    }
}