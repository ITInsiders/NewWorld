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

        public double Likes => review.Count(x => x.ValueLike == 1);
        public double Dislikes => review.Count(x => x.ValueLike == 2);
        public double Checks => review.Count(x => x.Checkin == 1);
        public double Rating => (Likes + Dislikes == 0.0) ? 0.0 : Math.Round(10.0 * Likes / (Likes + Dislikes), 1);

        public string MainPhoto => photos.FirstOrDefault(x => x.Main)?.SRC;

        public List<PlacePhotoDTO> photos => PlacePhotoServices.GetAll().Where(x => x.PlaceId == Id).ToList();
        public List<ReviewDTO> review => ReviewServices.GetAll().Where(x => x.PlaceId == Id).ToList();
    }
}
