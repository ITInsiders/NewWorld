using System.Collections.Generic;
using System.Linq;

using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class PlacePhotoServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(PlacePhotoDTO placePhotoDTO)
        {
            PlacePhoto placePhoto = MapperTransform<PlacePhoto, PlacePhotoDTO>.ToEntity(placePhotoDTO);
            Database.PlacePhotos.Create(placePhoto);
            Database.Save();
        }
        public static List<PlacePhotoDTO> GetAll()
        {
            List<PlacePhoto> placePhoto = Database.PlacePhotos.GetAll().ToList();
            return MapperTransform<PlacePhoto, PlacePhotoDTO>.ToModelCollection(placePhoto);
        }
        public static PlacePhotoDTO Get(int id)
        {
            PlacePhoto placePhoto = Database.PlacePhotos.Get(id);
            return MapperTransform<PlacePhoto, PlacePhotoDTO>.ToModel(placePhoto);
        }

        public static void Update(PlacePhotoDTO placePhotoDTO)
        {
            PlacePhoto placePhoto = Database.PlacePhotos.Get(placePhotoDTO.Id);
            placePhoto.SRC = placePhotoDTO.SRC;
            placePhoto.Main = placePhotoDTO.Main;
            Database.PlacePhotos.Update(placePhoto);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.PlacePhotos.Delete(id);
            Database.Save();
        }
    }
}
