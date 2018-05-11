using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NW.BL.DTO;
using NW.BL.Services;
using NW.BL.Extensions;

namespace NW.PL
{
    public class Identity
    {
        private HttpContext HC => HttpContext.Current;
        public UserDTO user = null;
        public CryptMD5 cryptMD5 = new CryptMD5();

        public bool Authentication(string Login, string Password)
        {
            if (HC.Request.Cookies["User"] != null) { HC.Response.Cookies["User"].Expires = DateTime.Now.AddYears(-1); user = null; }

            bool Answer = CheckUserData(Login, Password);
            if (Answer)
            {
                HttpCookie User = new HttpCookie("User");
                User.Expires = DateTime.Now.AddYears(1);

                User["Login"] = Login.ToLower();
                User["Password"] =  Password;

                HC.Response.Cookies.Add(User);
            }
            return Answer;
        }

        public bool isAuthentication
        {
            get
            {
                if (HC.Request.Cookies["User"] == null) { user = null; return false; }
                else
                {
                    string Login = HC.Request.Cookies["User"]["Login"].ToLower();
                    string Password = HC.Request.Cookies["User"]["Password"];
                    return CheckUserData(Login, Password);
                }
            }
        }

        public void ResetAuthentication()
        {
            if (HC.Request.Cookies["User"] != null)
            { HC.Response.Cookies["User"].Expires = DateTime.Now.AddYears(-1); user = null; }
        }

        public bool CheckUserData(string Login, string Password)
        {
            this.user = UserServices.GetAll().FirstOrDefault(x => x.Login == Login && x.Password == Password);
            return user != null;
        }
    }
}