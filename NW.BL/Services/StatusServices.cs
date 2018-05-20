using System.Collections.Generic;
using System.Linq;
using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class StatusServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(StatusDTO statusDTO)
        {
            Status status = MapperTransform<Status, StatusDTO>.ToEntity(statusDTO);
            Database.Statuses.Create(status);
            Database.Save();
        }
        public static List<StatusDTO> GetAll()
        {
            List<Status> status = Database.Statuses.GetAll().ToList();
            return MapperTransform<Status, StatusDTO>.ToModelCollection(status);
        }
        public static StatusDTO Get(int id)
        {
            Status status = Database.Statuses.Get(id);
            return MapperTransform<Status, StatusDTO>.ToModel(status);
        }

        public static void Update(StatusDTO statusDTO)
        {
            Status status = Database.Statuses.Get(statusDTO.Id);
            status.Title = statusDTO.Title;
            Database.Statuses.Update(status);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Statuses.Delete(id);
            Database.Save();
        }
    }
}
