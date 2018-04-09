using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.BL.Services;
using NW.PL.Models;

namespace NW.PL.Controllers
{
    public class HomeController : Controller
    {
        PageInfo page = new PageInfo("Home");

        public ActionResult Home()
        {
            ViewBag.Page = page.setView("Home").setTitle("NEW WORLD");
            return View();
        }
    }
}