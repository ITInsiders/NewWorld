using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NW.BL.DTO
{
    public class UserPhotoDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SRC { get; set; }
        public bool MainPhoto { get; set; }
    }
}
