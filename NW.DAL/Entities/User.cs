using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
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

        public virtual List<Review> Review { get; set; }
        public virtual List<UserPhoto> UserPhoto { get; set; }
        public virtual UserVerification UserVerification { get; set; }
        public virtual List<Place> Places { get; set; }

    }

   
}