using System.Collections.Generic;
using System.Linq;

using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class PlaceService
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(PlaceDTO placeDTO)
        {
            Place place = MapperTransform<Place, PlaceDTO>.ToEntity(placeDTO);
            Database.Places.Create(place);
            Database.Save();
        }
        public static List<PlaceDTO> GetAll()
        {
            List<Place> place = Database.Places.GetAll().ToList();
            return MapperTransform<Place, PlaceDTO>.ToModelCollection(place);
        }
        public static PlaceDTO Get(int id)
        {
            Place place = Database.Places.Get(id);
            return MapperTransform<Place, PlaceDTO>.ToModel(place);
        }

        public static void Update(PlaceDTO placeDTO)
        {
            Place place = Database.Places.Get(placeDTO.Id);
            place.Name = placeDTO.Name;
            place.Longitude = placeDTO.Longitude;
            place.Latitude = placeDTO.Latitude;
            place.Tags = placeDTO.Tags;
            place.WorkingHour = placeDTO.WorkingHour;
            place.Description = placeDTO.Description;
            place.Address = placeDTO.Address;
            place.Site = placeDTO.Site;
            place.Phone = placeDTO.Phone;
            place.Creater = placeDTO.Creater;
            place.DateCreate = placeDTO.DateCreate;
            Database.Places.Update(place);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Places.Delete(id);
            Database.Save();
        }
    }
}
