using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NW.BL.DTO;
using NW.BL.Infrastructure;
using NW.DAL.Repositories;
using NW.DAL.Entities;
using NW.BL.Extensions;

namespace NW.BL.Services
{
    public class UserVerificationServices
    {
        private static EFUnitOfWork Database = EFUnitOfWork.I;

        public static void Create(UserVerificationDTO userVerificationDTO)
        {
            UserVerification userVerification = MapperTransform<UserVerification, UserVerificationDTO>.ToEntity(userVerificationDTO);
            Database.UserVerifications.Create(userVerification);
            Database.Save();
        }
        public static List<UserVerificationDTO> GetAll()
        {
            List<UserVerification> userVerification = Database.UserVerifications.GetAll().ToList();
            return MapperTransform<UserVerification, UserVerificationDTO>.ToModelCollection(userVerification);
        }
        public static UserVerificationDTO Get(int id)
        {
            UserVerification userVerification = Database.UserVerifications.Get(id);
            return MapperTransform<UserVerification, UserVerificationDTO>.ToModel(userVerification);
        }

        public static void Update(UserVerificationDTO userVerificationDTO)
        {
            UserVerification userVerification = Database.UserVerifications.Get(userVerificationDTO.Id);
            userVerification.Email = userVerificationDTO.Email;
            userVerification.PhoneNumber = userVerificationDTO.PhoneNumber;
            Database.UserVerifications.Update(userVerification);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.UserVerifications.Delete(id);
            Database.Save();
        }
    }
}
