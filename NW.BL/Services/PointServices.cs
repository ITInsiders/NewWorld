using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class PointServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(PointDTO pointDTO)
        {
            Point point = MapperTransform<Point, PointDTO>.ToEntity(pointDTO);
            Database.Points.Create(point);
            Database.Save();
        }
        public static List<PointDTO> GetAll()
        {
            List<Point> point = Database.Points.GetAll().ToList();
            return MapperTransform<Point, PointDTO>.ToModelCollection(point);
        }
        public static PointDTO Get(int id)
        {
            Point point = Database.Points.Get(id);
            return MapperTransform<Point, PointDTO>.ToModel(point);
        }

        public static void Update(PointDTO pointDTO)
        {
            Point point = Database.Points.Get(pointDTO.Id);
            point.Latitude = pointDTO.Latitude;
            point.Address = pointDTO.Address;
            point.Longitude = pointDTO.Longitude;
            point.QuestId = pointDTO.QuestId;
            point.Task = pointDTO.Task;
            Database.Points.Update(point);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Points.Delete(id);
            Database.Save();
        }
    }
}
