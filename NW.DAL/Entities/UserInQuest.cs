using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("UserInQuests")]
    public class UserInQuest
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? QuestId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int StatusId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [ForeignKey("QuestId")]
        public virtual Quest Quest { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
    }
}
