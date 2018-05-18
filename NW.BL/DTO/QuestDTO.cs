using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NW.BL.Services;

namespace NW.BL.DTO
{
    public class QuestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartQuest { get; set; }
        public int? LimitOfPeople { get; set; }
        public int Creater { get; set; }
        public DateTime DateCreate { get; set; }
        public string SRC { get; set; }

        public List<UserInQuestDTO> userInQuestDTO => UserInQuestServices.GetAll().Where(x => x.QuestId == Id).ToList();
        public List<PrizeDTO> prizeDTO => PrizeServices.GetAll().Where(x => x.QuestId == Id).ToList();
        public List<PointDTO> pointDTO => PointServices.GetAll().Where(x => x.QuestId == Id).ToList();

        public int countPeople => userInQuestDTO.Count(x=> x.StatusId == 1);

    }
}
