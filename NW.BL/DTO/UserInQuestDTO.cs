using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class UserInQuestDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int StatusId { get; set; }
    }
}
