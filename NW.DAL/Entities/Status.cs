using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Statuses")]
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual List<UserInQuest> UserInQuest { get; set; }
    }
}
