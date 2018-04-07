using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class PlacePhotoDTO
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public string SRC { get; set; }
        public bool Main { get; set; }
    }
}
