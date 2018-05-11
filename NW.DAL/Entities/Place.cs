using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Places")]
    public class Place
    {
        [Key]
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

        [ForeignKey("Creater")]
        public virtual User User { get; set; }

        public virtual List<Review> PlaceReview { get; set; }
        public virtual List<PlacePhoto> PlacePhoto { get; set; }

    }
}