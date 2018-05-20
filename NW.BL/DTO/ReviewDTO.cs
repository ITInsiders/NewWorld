using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PlaceId { get; set; }
        public string Comment { get; set; }
        public int ValueLike { get; set; } //лайк-1 дизлайк-2 
        public int Checkin { get; set; } //чекин
        public DateTime Date { get; set; } 
    }
}
