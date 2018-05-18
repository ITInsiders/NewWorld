using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NW.BL.Services;

namespace NW.BL.DTO
{
    public class StatusDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<UserInQuestDTO> userInQuestDTO => UserInQuestServices.GetAll().Where(x => x.StatusId == Id).ToList();
    }
}
