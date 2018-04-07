using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace NW.BL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime? DateOfLastVisit { get; set; }
        public DateTime? DateOfLastChange { get; set; }
        public double? Rating { get; set; }
        public int Access { get; set; } // доступы: 0 - простой пользоваель, 1 - администратор

    }

    public class UserRegistration: UserDTO
    {
        public string RePassword { get; set; }
    }
}
