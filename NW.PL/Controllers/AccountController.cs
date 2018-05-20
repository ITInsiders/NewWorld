using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.PL.Models;
using NW.BL.DTO;
using NW.BL.Services;
using NW.BL.Extensions;

namespace NW.PL.Controllers
{
    public class AccountController : Controller
    {
        PageInfo pageInfo = PageInfo.Create("Account");
        Identity Identity = new Identity();
        CryptMD5 cryptMD5 = new CryptMD5();

        public ActionResult Account()
        {
            ViewBag.Page = pageInfo.setView("Account");
            return View();
        }

        [HttpGet]
        public ActionResult Entry()
        {
            ViewBag.Page = pageInfo.setView("Entry");
            return View();
        }

        [HttpPost]
        public ActionResult Entry(Entry Model)
        {
            if (ModelState.IsValid)
            {
                if (!Identity.Authentication(Model.Login, cryptMD5.GetHash(Model.Password))) ModelState.AddModelError("Password", "Пароль не верный");
            }
            return Redirect("/Account/Entry");
        }

        [HttpPost]
        public ActionResult Registration(UserRegistration Model)
        {
            if (ModelState.IsValid)
            {
                UserDTO user = UserServices.GetAll().FirstOrDefault(x => x.Login == Model.Login);
                if (user != null) { return Redirect("/Account/Account"); }
                else
                {
                    if (Model.Password != Model.RePassword || Model.Password == "") return Redirect("/Account/Account");
                    else
                    {
                        Model.Profile.DateOfRegistration = DateTime.Now;
                        Model.Profile.Access = 0;
                        Model.Login = Model.Login.ToLower();
                        Model.Password = cryptMD5.GetHash(Model.Password);
                        UserServices.Create((UserDTO)Model.Profile);
                        if (Identity.Authentication(Model.Login, Model.Password)) return Redirect("/Home");
                    }
                }
            }
            return Redirect("/Account/Account");
        }

        public ActionResult Exit()
        {
            Identity.ResetAuthentication();
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public JsonResult Authentication()
        {
            return Json(Identity.isAuthentication, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ECheckLogin(string Login)
        {
            var result = UserServices.GetAll().Any(x => x.Login == Login);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult RCheckLogin(string Login)
        {
            var result = !UserServices.GetAll().Any(x => x.Login == Login);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Profile()
        {
            ViewBag.Page = pageInfo.setView("Profile");
            if (Identity.isAuthentication)
            {
                List<UserPhotoDTO> userPhoto = UserPhotoServices.GetAll().Where(x => x.UserId == Identity.user.Id && x.MainPhoto == true).ToList();
                ViewBag.userPhoto = userPhoto;
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadPhoto(HttpPostedFileBase upload)
        {
            if (upload != null && Identity.isAuthentication)
            {
                string dir = Server.MapPath("~/Resources/Images/Users/" + Identity.user.Id);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string[] dirs = Directory.GetFiles(Server.MapPath("~/Resources/Images/Users/" + Identity.user.Id), "*");

                string type = upload.FileName.Split('.').Last();

                string src = "/Resources/Images/Users/" + Identity.user.Id + "/" + cryptMD5.GetHash(dirs.Length.ToString()) + "." + type;
                string path = Server.MapPath("~" + src);

                upload.SaveAs(path);

                UserPhotoDTO userPhoto = UserPhotoServices.GetAll().FirstOrDefault(x => x.MainPhoto && x.UserId == Identity.user.Id);
                if (userPhoto != null)
                {
                    userPhoto.MainPhoto = false;
                    UserPhotoServices.Update(userPhoto);
                }

                userPhoto = new UserPhotoDTO();
                userPhoto.MainPhoto = true;
                userPhoto.SRC = src;
                userPhoto.UserId = Identity.user.Id;
                UserPhotoServices.Create(userPhoto);
            }

            return RedirectToAction("Profile");
        }

    }
}