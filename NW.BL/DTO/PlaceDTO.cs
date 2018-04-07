using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using NW.BL.Services;

namespace NW.BL.DTO
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }//долгота
        public double Latitude { get; set; }//широта
        public string Tags { get; set; }//категория, теги
        public string WorkingHour { get; set; }// время работы 
        public string Description { get; set; } // описание
        public string Address { get; set; } //адрес
        public string Site { get; set; }//сайт
        public string Phone { get; set; } // номер телефона
        public int? Creater { get; set; } // создатель
        public DateTime DateCreate { get; set; } // дата создания

        public List<PlacePhotoDTO> photos => PlacePhotoServices.GetAll().Where(x => x.PlaceId == Id).ToList();
    }
}
