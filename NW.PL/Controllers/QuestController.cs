using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.PL.Models;

namespace NW.PL.Controllers
{
    public class QuestController : Controller
    {
        PageInfo page = new PageInfo("Quest");
        
        public ActionResult Home()
        {
            ViewBag.Page = page.setView("Home").setTitle("Home - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Page = page.setView("Search").setTitle("Search - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            return View();
        }
    }
}