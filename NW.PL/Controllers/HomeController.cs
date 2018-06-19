using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.BL.Services;
using NW.PL.Models;
using NW.BL.DTO;
using NW.BL.Services;
using NW.BL.Extensions;

namespace NW.PL.Controllers
{
    public class HomeController : Controller
    {
        PageInfo page = new PageInfo("Home");

        public ActionResult Home()
        {
            ViewBag.Page = page.setView("Home").setTitle("NEW WORLD");
            ViewBag.Message = TempData["Message"];

            List<PlaceDTO> placeDTO = PlaceService.GetAll().OrderByDescending(x => x.Rating).ToList();
            List<QuestDTO> questDTO = QuestServices.GetAll().OrderByDescending(x => x.StartQuest).ThenBy(x => x.countPeople).ThenBy(x => x.Name).ToList();

            ViewBag.placeDTO = placeDTO;
            ViewBag.questDTO = questDTO;
            return View();
        }
    }
}