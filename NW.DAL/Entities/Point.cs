using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Points")]
    public class Point
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public double Longitude { get; set; }//долгота
        public double Latitude { get; set; }//широта
        public int QuestId { get; set; }
        public string Task { get; set; }

        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }

        public virtual List<Answer> Answer { get; set; }
    }
}
