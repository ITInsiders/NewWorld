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
        public class Place
        {
            public int Id { get; set; }
            public double[] Position { get; set; }
            public string Address { get; set; }
        }

        public List<JsonUser> Users { get; set; }
        public List<Place> Places { get; set; }

        public JsonQuestUsers(QuestGame quest) : base(quest)
        {
            Users = quest.users.Where(x => !x.isCreator).Select(x => new JsonUser(x)).ToList();
            Places = quest.Tasks.Select(x => new Place() { Id = x.Id, Address = x.Answer, Position = x.Position }).ToList();
        }
    }
}