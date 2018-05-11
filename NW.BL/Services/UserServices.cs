using System.Collections.Generic;
using System.Linq;

using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class UserServices
    {
        private static EFUnitOfWork Database => EFUnitOfWork.I;

        public static void Create(UserDTO userDTO)
        {
            User user = MapperTransform<User, UserDTO>.ToEntity(userDTO);
            Database.Users.Create(user);
            Database.Save();
        }
        public static List<UserDTO> GetAll()
        {
            List<User> user = Database.Users.GetAll().ToList();
            return MapperTransform<User, UserDTO>.ToModelCollection(user);
        }
        public static UserDTO Get(int id)
        {
            User user = Database.Users.Get(id);
            return MapperTransform<User, UserDTO>.ToModel(user);
        }

        public static void Update(UserDTO userDTO)
        {
            User user = Database.Users.Get(userDTO.Id);
            user.Login = userDTO.Login;
            user.Password = userDTO.Password;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.MiddleName = userDTO.MiddleName;
            user.Email = userDTO.Email;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.DateOfBirth = userDTO.DateOfBirth;
            user.DateOfLastChange = userDTO.DateOfLastChange;
            user.Rating = userDTO.Rating;
            user.Access = userDTO.Access;
            Database.Users.Update(user);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Users.Delete(id);
            Database.Save();
        }
    }
}
