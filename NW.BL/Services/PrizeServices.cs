using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class PrizeServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(PrizeDTO prizeDTO)
        {
            Prize prize = MapperTransform<Prize, PrizeDTO>.ToEntity(prizeDTO);
            Database.Prizes.Create(prize);
            Database.Save();
        }
        public static List<PrizeDTO> GetAll()
        {
            List<Prize> prize = Database.Prizes.GetAll().ToList();
            return MapperTransform<Prize, PrizeDTO>.ToModelCollection(prize);
        }
        public static PrizeDTO Get(int id)
        {
            Prize prize = Database.Prizes.Get(id);
            return MapperTransform<Prize, PrizeDTO>.ToModel(prize);
        }

        public static void Update(PrizeDTO prizeDTO)
        {
            Prize prize = Database.Prizes.Get(prizeDTO.Id);
            prize.Name = prizeDTO.Name;
            prize.QuestId = prizeDTO.QuestId;
            prize.MaxPlace = prizeDTO.MaxPlace;
            prize.MinPlace = prizeDTO.MinPlace;
            Database.Prizes.Update(prize);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Prizes.Delete(id);
            Database.Save();
        }
    }
}
