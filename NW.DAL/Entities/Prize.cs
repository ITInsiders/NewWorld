using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Prizes")]
    public class Prize
    {
        [Key]
        public int Id { get; set; }
        public int QuestId { get; set; }
        public string Name { get; set; }
        public int MinPlace { get; set; }
        public int MaxPlace { get; set; }

        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }
    }
}
