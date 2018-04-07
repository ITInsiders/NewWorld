using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NW.DAL.Entities
{
    public class UserVerification
    {
        [Key]
        [ForeignKey("User")]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public virtual User User { get; set; }
    }
}