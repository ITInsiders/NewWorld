using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NW.BL.DTO;

namespace NW.PL.Models.Quest
{
    public class JsonQuest
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public JsonQuest(QuestGame quest)
        {
            Id = quest.Id;
            Name = quest.Quest.Name;
        }
    }

    public class JsonQuestUsers : JsonQuest
    {
        public List<JsonUser> Users { get; set; }

        public JsonQuestUsers(QuestGame quest) : base(quest)
        {
            Users = quest.users.Select(x => new JsonUser(x)).ToList();
        }
    }
}