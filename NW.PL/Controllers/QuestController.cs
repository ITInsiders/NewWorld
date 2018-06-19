using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.PL.Models;
using NW.BL.Services;
using NW.BL.DTO;
using NW.PL.Models;
using System.Drawing;

namespace NW.PL.Controllers
{
    public class QuestController : Controller
    {
        PageInfo page = new PageInfo("Quest");
        Identity Identity = new Identity();

        public ActionResult Home()
        {
            ViewBag.Page = page.setView("Home").setTitle("Home - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;
            return View();
        }

        public ActionResult Search(int module)
        {
            ViewBag.Page = page.setView("Search").setTitle("Search - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            ViewBag.id = module;
            return View();
        }

        public ActionResult Game(int module)
        {
            if (Identity.isAuthentication)
            {
                ViewBag.Page = page.setView("Game").setTitle("Game - Quest / NEW WORLD");
                ViewBag.Method = HttpContext.Request.HttpMethod;

                QuestDTO questDTO = QuestServices.Get(module);
                if (questDTO == null) return HttpNotFound();
                List<PointDTO> pointDTO = PointServices.GetAll().Where(x => x.QuestId == module).ToList();
                ViewBag.Point = pointDTO;
                return View(questDTO);
            }
            else return Redirect("Error");
        }

        public ActionResult Result(int module)
        {
            ViewBag.Page = page.setView("Result").setTitle("Result - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            QuestDTO questDTO = QuestServices.Get(module);
            if (questDTO == null) return HttpNotFound();
            List<PrizeDTO> prizeDTO = PrizeServices.GetAll().Where(x=>x.QuestId == module).ToList();
            ViewBag.prizeDTO = prizeDTO;

            return View(questDTO);
        }

        public ActionResult InformQuest(int module)
        {
            ViewBag.Page = page.setView("InformQuest").setTitle("Information - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            QuestDTO questDTO = QuestServices.Get(module);
            if (questDTO == null) return HttpNotFound();

            bool userStart = false;
            bool can = false;
            List<PrizeDTO> prize = PrizeServices.GetAll().Where(x => x.QuestId == questDTO.Id).ToList();
            if (Identity.isAuthentication)
            {
                userStart = !UserInQuestServices.GetAll().Any(x => x.UserId == Identity.user.Id && x.StatusId == 1 && questDTO.Id == x.QuestId);
                //если false то не может участвовать


                List<UserInQuestDTO> userInQuest = UserInQuestServices.GetAll().Where(x => x.UserId == Identity.user.Id && x.StatusId == 1).ToList();
                can = QuestServices.GetAll()
                    .Any(x => userInQuest.Any(s => x.Id == s.QuestId) &&
                    Math.Abs(questDTO.StartQuest.Subtract(x.StartQuest).TotalHours) < 5);
                //если true то не может участвовать
            }

            ViewBag.userStart = userStart;
            ViewBag.can = can;
            ViewBag.prize = prize;
            ViewBag.countPeople = questDTO.countPeople;


            return View(questDTO);
        }
        public JsonResult SearchLines()
        {
            string Search = Request.Form["Search"];
            int id = Convert.ToInt32(Request.Form["id"]);

            List<QuestDTO> quest = QuestServices.GetAll().Where(x => x.Name.ToLower().Contains(Search.ToLower())).ToList();
            List<UserInQuestDTO> userInQuest = UserInQuestServices.GetAll();
            List<SearchLine> blocks = new List<SearchLine>();
            int count = 0;
            if (id == 1)
            {
                blocks.AddRange(quest
                .Where(x => x.StartQuest > DateTime.Now && x.StartQuest < DateTime.Now.AddDays(2)).Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.StartQuest).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));

            }
            else if (id == 2)
            {
                blocks.AddRange(quest
               .Where(x => x.StartQuest > DateTime.Now).Where(x => x.LimitOfPeople > x.countPeople)
               .OrderByDescending(x => x.countPeople).ThenBy(x => x.Name)
               .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 3)
            {
                blocks.AddRange(quest
                .Where(x => x.StartQuest > DateTime.Now).Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 4)
            {
                blocks.AddRange(quest
                .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.Id).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 5 && Identity.isAuthentication)
            {
                blocks.AddRange(quest
                .Where(x => x.Creater == Identity.user.Id)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 6 && Identity.isAuthentication)
            {
                blocks.AddRange(quest
                .Where(x => x.userInQuestDTO.Any(s => s.UserId == Identity.user.Id))
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else blocks.Select(x => new SearchLine() { Type = "", Value = "" });
            return Json(blocks);
        }
        public JsonResult SearchBlocks()
        {
            return Json(QuestsObj());
        }

        public List<QuestDTO> QuestsObj()
        {
            string Search = Request.Form["Search"];
            int id = Convert.ToInt32(Request.Form["id"]);
            List<QuestDTO> quest = QuestServices.GetAll().Where(x => x.Name.ToLower().Contains(Search.ToLower())).ToList();
            if (id == 1)
            {
                quest = quest.ToList().Where(x => x.StartQuest > DateTime.Now && x.StartQuest < DateTime.Now.AddDays(2))
                .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.StartQuest).ThenBy(x => x.Name).ToList();
            }
            else if (id == 2)
            {
                quest = quest.Where(x => x.StartQuest > DateTime.Now)
                    .Where(x => x.LimitOfPeople > x.countPeople)
                    .OrderByDescending(x => x.countPeople).ThenBy(x => x.Name).ToList();

            }
            else if (id == 3)
            {
                quest = quest.Where(x => x.StartQuest > DateTime.Now)
                    .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name).ToList();
            }
            else if (id == 4)
            {
                quest = quest.Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.Id).ThenBy(x => x.Name).ToList();
            }
            else if (id == 5 && Identity.isAuthentication)
            {
                quest = quest.Where(x => x.Creater == Identity.user.Id)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name).ToList();
            }
            else if (id == 6 && Identity.isAuthentication)
            {
                quest = quest.Where(x => x.userInQuestDTO.Any(s => s.UserId == Identity.user.Id))
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name).ToList();
            }
            return quest;
        }
        public ActionResult Participate(int module)
        {
            if (Identity.isAuthentication)
            {
                UserInQuestDTO userInQuestDTO = new UserInQuestDTO();
                userInQuestDTO.UserId = Identity.user.Id;
                userInQuestDTO.QuestId = module;
                userInQuestDTO.StatusId = 1;
                UserInQuestServices.Create(userInQuestDTO);
            }


            return RedirectToAction("InformQuest/" + module);
        }
        public ActionResult Refusal(int module)
        {
            if (Identity.isAuthentication)
            {
                UserInQuestDTO userInQuestDTO = UserInQuestServices.GetAll().FirstOrDefault(x => x.QuestId == module && x.UserId == Identity.user.Id && x.StatusId == 1);
                userInQuestDTO.StatusId = 2;
                UserInQuestServices.Update(userInQuestDTO);
            }
            return RedirectToAction("InformQuest/" + module);
        }
        public JsonResult OnZonesLoad()
        {
            List<QuestDTO> quest = QuestsObj();

            List<ZonesLoad> zonesLoad = new List<ZonesLoad>();
            double argLat = 0, ardLong = 0;

            foreach (QuestDTO obj in quest)
            {
                ZonesLoad zlQuest = new ZonesLoad();
                argLat = 0; ardLong = 0;
                foreach (PointDTO point in obj.pointDTO)
                {
                    argLat += point.Latitude;
                    ardLong += point.Longitude;
                }
                zlQuest.ArgLatitude = (obj.pointDTO.Count == 0) ? 0 : argLat / obj.pointDTO.Count;
                zlQuest.ArgLongtude = (obj.pointDTO.Count == 0) ? 0 : ardLong / obj.pointDTO.Count;
                zlQuest.Name = obj.Name;
                obj.pointDTO.ForEach(x => zlQuest.coordinates.Add(new double[2] { x.Latitude, x.Longitude }));

                Random randonGen = new Random();
                Color randomColor = Color.FromArgb(70, randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                zlQuest.fillColor = "#" + randomColor.R.ToString("X2") + randomColor.G.ToString("X2") + randomColor.B.ToString("X2") + randomColor.A.ToString("X2");
                zonesLoad.Add(zlQuest);


                //ZonesLoad.Features feature = new ZonesLoad.Features();
                //  feature.id = obj.Id;

                //  ZonesLoad.Geometry geometry = new ZonesLoad.Geometry();
                //  obj.pointDTO.ForEach(x => geometry.coordinates[0].Add(new double[2] { x.Latitude, x.Longitude }));
                //  feature.geometry = geometry;

                //  Random randonGen = new Random();
                //  Color randomColor = Color.FromArgb(255, randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
                //  ZonesLoad.Options options = new ZonesLoad.Options();
                //  options.fillColor = "#" + randomColor.R.ToString("X2") + randomColor.G.ToString("X2") + randomColor.B.ToString("X2") + randomColor.A.ToString("X2");
                //  feature.options = options;

                //  ZonesLoad.Properties properties = new ZonesLoad.Properties();
                //  properties.name = obj.Name;
                //  properties.dateTime = obj.StartQuest;
                //  feature.properties = properties;

                //  zonesLoad.features.Add(feature);
            }

            return Json(zonesLoad);
        }

        public JsonResult OnZonesLoadOneQuest(int module)
        {
            QuestDTO quest = QuestServices.Get(module);

            ZonesLoad zonesLoad = new ZonesLoad();
            double argLat = 0, ardLong = 0;
            double Radius = 0;

            foreach (PointDTO point in quest.pointDTO)
            {
                argLat += point.Latitude;
                ardLong += point.Longitude;
            }

            argLat = (quest.pointDTO.Count == 0) ? 0 : argLat / quest.pointDTO.Count;
            ardLong = (quest.pointDTO.Count == 0) ? 0 : ardLong / quest.pointDTO.Count;

            foreach (PointDTO point in quest.pointDTO)
            {
                double TRadius = Math.Sqrt(Math.Pow(argLat - point.Latitude, 2) + Math.Pow(ardLong - point.Longitude, 2));
                Radius = Radius > TRadius ? Radius : TRadius;
            }

            Radius += 0.002;
            zonesLoad.Bounds = new double[2][] { new double[2] { argLat + Radius, ardLong - Radius }, new double[2] { argLat - Radius, ardLong + Radius } };
            zonesLoad.ArgLatitude = argLat;
            zonesLoad.ArgLongtude = ardLong;
            zonesLoad.Name = quest.Name;
            quest.pointDTO.ForEach(x => zonesLoad.coordinates.Add(new double[2] { x.Latitude, x.Longitude }));

            Random randonGen = new Random();
            Color randomColor = Color.FromArgb(70, randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
            zonesLoad.fillColor = "#" + randomColor.R.ToString("X2") + randomColor.G.ToString("X2") + randomColor.B.ToString("X2") + randomColor.A.ToString("X2");

            return Json(zonesLoad);
        }
        
    }
}