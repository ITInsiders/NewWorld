using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Quests")]
    public class Quest
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartQuest { get; set; }
        public int? LimitOfPeople { get; set; }

        [ForeignKey("User")]
        public int Creater { get; set; }
        public virtual User User { get; set; }

        public DateTime DateCreate { get; set; }
        public string SRC { get; set; }

        public virtual List<UserInQuest> UserInQuest { get; set; }
        public virtual List<Prize> Prize { get; set; }
        public virtual List<Point> Point { get; set; }
    }
}
