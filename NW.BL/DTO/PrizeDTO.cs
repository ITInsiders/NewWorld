using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class PrizeDTO
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public string Name { get; set; }
        public int MinPlace { get; set; }
        public int MaxPlace { get; set; }
    }
}
