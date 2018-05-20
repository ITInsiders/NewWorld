using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class UserInQuestServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(UserInQuestDTO userInQuestDTO)
        {
            UserInQuest userInQuest = MapperTransform<UserInQuest, UserInQuestDTO>.ToEntity(userInQuestDTO);
            Database.UserInQuests.Create(userInQuest);
            Database.Save();
        }
        public static List<UserInQuestDTO> GetAll()
        {
            List<UserInQuest> userInQuest = Database.UserInQuests.GetAll().ToList();
            return MapperTransform<UserInQuest, UserInQuestDTO>.ToModelCollection(userInQuest);
        }
        public static UserInQuestDTO Get(int id)
        {
            UserInQuest userInQuest = Database.UserInQuests.Get(id);
            return MapperTransform<UserInQuest, UserInQuestDTO>.ToModel(userInQuest);
        }

        public static void Update(UserInQuestDTO userInQuestDTO)
        {
            UserInQuest userInQuest = Database.UserInQuests.Get(userInQuestDTO.Id);
            userInQuest.UserId = userInQuestDTO.UserId;
            userInQuest.QuestId = userInQuestDTO.QuestId;
            userInQuest.ExpirationDate = userInQuestDTO.ExpirationDate;
            userInQuest.StatusId = userInQuestDTO.StatusId;
            Database.UserInQuests.Update(userInQuest);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.UserInQuests.Delete(id);
            Database.Save();
        }
    }
}
