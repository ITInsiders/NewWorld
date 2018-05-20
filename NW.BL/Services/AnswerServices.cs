using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class AnswerServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(AnswerDTO answerDTO)
        {
            Answer answer = MapperTransform<Answer, AnswerDTO>.ToEntity(answerDTO);
            Database.Answers.Create(answer);
            Database.Save();
        }
        public static List<AnswerDTO> GetAll()
        {
            List<Answer> answer = Database.Answers.GetAll().ToList();
            return MapperTransform<Answer, AnswerDTO>.ToModelCollection(answer);
        }
        public static AnswerDTO Get(int id)
        {
            Answer answer = Database.Answers.Get(id);
            return MapperTransform<Answer, AnswerDTO>.ToModel(answer);
        }

        public static void Update(AnswerDTO answerDTO)
        {
            Answer answer = Database.Answers.Get(answerDTO.Id);
            answer.PointId = answerDTO.PointId;
            answer.UserId = answerDTO.UserId;
            answer.GeoSuccess = answerDTO.GeoSuccess;
            answer.Message = answerDTO.Message;
            Database.Answers.Update(answer);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Answers.Delete(id);
            Database.Save();
        }
    }
}
