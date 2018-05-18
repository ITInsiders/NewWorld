using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public int PointId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool GeoSuccess { get; set; }

        [ForeignKey("PointId")]
        public virtual Point Point { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
