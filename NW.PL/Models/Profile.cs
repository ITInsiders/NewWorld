using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using NW.BL.DTO;

namespace NW.PL.Models
{
    public class Profile
    {
        public class Entry
        {
            public UserDTO Profile = new UserDTO();

            [Required(ErrorMessage = "Введите логин")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Логин должен состоять от 3 до 30 символов")]
            [RegularExpression(@"[aA-zZ]{1}[aA-zZ0-9_-]{2,29}", ErrorMessage = "Не правильно введен логин")]
            [System.Web.Mvc.Remote("ECheckLogin", "Account", ErrorMessage = "Данного логина не существует")]
            public string Login { get { return Profile.Login; } set { Profile.Login = value; } }

            [Required(ErrorMessage = "Введите пароль")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Пароль должен состоять от 3 до 30 символов")]
            public string Password { get { return Profile.Password; } set { Profile.Password = value; } }
        }

        public class Registration
        {
            public UserDTO Profile = new UserDTO();

            [Required(ErrorMessage = "Введите логин")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Логин должен состоять от 3 до 30 символов")]
            [RegularExpression(@"[aA-zZ]{1}[aA-zZ0-9_-]{2,29}", ErrorMessage = "Не правильно введен логин")]
            [System.Web.Mvc.Remote("RCheckLogin", "Account", ErrorMessage = "Данный логин уже используется")]
            public string Login { get { return Profile.Login; } set { Profile.Login = value; } }

            [Required(ErrorMessage = "Введите пароль")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Пароль должен состоять от 3 до 30 символов")]
            public string Password { get { return Profile.Password; } set { Profile.Password = value; } }

            [Required(ErrorMessage = "Повторите пароль")]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string RePassword { get; set; }
        }
    }
}