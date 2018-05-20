using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Reviews")]
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public string Comment { get; set; }
        public int ValueLike { get; set; } //лайк-1 дизлайк-2 
        public int Checkin { get; set; } //чекин
        public DateTime Date { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PlaceId")]
        public virtual Place Place { get; set; }
    }
}
