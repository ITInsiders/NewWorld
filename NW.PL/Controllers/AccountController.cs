﻿using System;
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
    }
}