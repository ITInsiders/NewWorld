using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NW.BL.Services;

namespace NW.BL.DTO
{
    public class PointDTO
    {
        public int Id { get; set; }
        public double Longitude { get; set; }//долгота
        public double Latitude { get; set; }//широта
        public int QuestId { get; set; }
        public string Task { get; set; }
        
        public List<AnswerDTO> answerDTO => AnswerServices.GetAll().Where(x => x.PointId == Id).ToList();

    }
}
