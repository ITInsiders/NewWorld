using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NW.DAL.Repositories;
using NW.DAL.Entities;
using NW.BL.Extensions;
using NW.BL.DTO;

namespace NW.BL.Services
{
    public class UserPhotoServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(UserPhotoDTO userPhotoDTO)
        {
            UserPhoto userPhoto = MapperTransform<UserPhoto, UserPhotoDTO>.ToEntity(userPhotoDTO);
            Database.UserPhotos.Create(userPhoto);
            Database.Save();
        }
        public static List<UserPhotoDTO> GetAll()
        {
            List<UserPhoto> userPhoto = Database.UserPhotos.GetAll().ToList();
            return MapperTransform<UserPhoto, UserPhotoDTO>.ToModelCollection(userPhoto);
        }
        public static UserPhotoDTO Get(int id)
        {
            UserPhoto userPhoto = Database.UserPhotos.Get(id);
            return MapperTransform<UserPhoto, UserPhotoDTO>.ToModel(userPhoto);
        }

        public static void Update(UserPhotoDTO userPhotoDTO)
        {
            UserPhoto userPhoto = Database.UserPhotos.Get(userPhotoDTO.Id);
            userPhoto.MainPhoto = userPhotoDTO.MainPhoto;
            userPhoto.SRC = userPhotoDTO.SRC;
            userPhoto.UserId = userPhotoDTO.UserId;
            Database.UserPhotos.Update(userPhoto);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.UserPhotos.Delete(id);
            Database.Save();
        }
    }
}
