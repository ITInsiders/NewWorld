using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("UserPhotos")]
    public class UserPhoto
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SRC { get; set; }
        public bool MainPhoto { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
