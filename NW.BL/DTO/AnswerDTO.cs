using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public int PointId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public bool GeoSuccess { get; set; }
    }
}
