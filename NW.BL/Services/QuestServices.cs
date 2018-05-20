using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class QuestServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(QuestDTO questDTO)
        {
            Quest quest = MapperTransform<Quest, QuestDTO>.ToEntity(questDTO);
            Database.Quests.Create(quest);
            Database.Save();
        }
        public static List<QuestDTO> GetAll()
        {
            List<Quest> quest = Database.Quests.GetAll().ToList();
            return MapperTransform<Quest, QuestDTO>.ToModelCollection(quest);
        }
        public static QuestDTO Get(int id)
        {
            Quest quest = Database.Quests.Get(id);
            return MapperTransform<Quest, QuestDTO>.ToModel(quest);
        }

        public static void Update(QuestDTO questDTO)
        {
            Quest quest = Database.Quests.Get(questDTO.Id);
            quest.Name = questDTO.Name;
            quest.Description = questDTO.Description;
            quest.Creater = questDTO.Creater;
            quest.DateCreate = questDTO.DateCreate;
            quest.LimitOfPeople = questDTO.LimitOfPeople;
            quest.StartQuest = questDTO.StartQuest;
            quest.SRC = questDTO.SRC;
            Database.Quests.Update(quest);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Quests.Delete(id);
            Database.Save();
        }
    }
}
