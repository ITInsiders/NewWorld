using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.PL.Models;
using NW.BL.Services;
using NW.BL.DTO;
using NW.PL.Models;

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

        public ActionResult InformQuest(int module)
        {
            ViewBag.Page = page.setView("InformQuest").setTitle("Information - Quest / NEW WORLD");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            QuestDTO questDTO = QuestServices.Get(module);
            if (questDTO == null) return HttpNotFound();

            QuestDTO obj = new QuestDTO()
            {
                Id = questDTO.Id,
                Name = questDTO.Name,
                Description = questDTO.Description,
                StartQuest = questDTO.StartQuest,
                LimitOfPeople = questDTO.LimitOfPeople,
                SRC = questDTO.SRC
            };
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


            return View(obj);
        }
        public JsonResult SearchLines()
        {
            string Search = Request.Form["Search"];
            int id = Convert.ToInt32(Request.Form["id"]);

            List<QuestDTO> quest = QuestServices.GetAll();
            List<UserInQuestDTO> userInQuest = UserInQuestServices.GetAll();
            List<SearchLine> blocks = new List<SearchLine>();
            int count = 0;
            if (id == 1)
            {
                blocks.AddRange(quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                .Where(x => x.StartQuest > DateTime.Now && x.StartQuest < DateTime.Now.AddDays(2)).Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.StartQuest).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));

            }
            else if (id == 2)
            {
                blocks.AddRange(quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
               .Where(x => x.StartQuest > DateTime.Now).Where(x => x.LimitOfPeople > x.countPeople)
               .OrderByDescending(x => x.countPeople).ThenBy(x => x.Name)
               .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 3)
            {
                blocks.AddRange(quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                .Where(x => x.StartQuest > DateTime.Now).Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else if (id == 4)
            {
                blocks.AddRange(quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.Id).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = "Квест", Value = x.Name }));
            }
            else blocks.Select(x => new SearchLine() { Type = "", Value = "" });
            return Json(blocks);
        }
        public JsonResult SearchBlocks()
        {
            string Search = Request.Form["Search"];
            int id = Convert.ToInt32(Request.Form["id"]);
            List<QuestDTO> quest = QuestServices.GetAll();
            if (id == 1)
            {
                quest = quest.Where(x => x.Name.ToLower().Contains(Search.ToLower())).ToList()
                .Where(x => x.StartQuest > DateTime.Now && x.StartQuest < DateTime.Now.AddDays(2))
                .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.StartQuest).ThenBy(x => x.Name).ToList();
            }
            else if (id == 2)
            {
                quest = quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                    .Where(x => x.StartQuest > DateTime.Now)
                    .Where(x => x.LimitOfPeople > x.countPeople)
                    .OrderByDescending(x => x.countPeople).ThenBy(x => x.Name).ToList();

            }
            else if (id == 3)
            {
                quest = quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                    .Where(x => x.StartQuest > DateTime.Now)
                    .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.DateCreate).ThenBy(x => x.Name).ToList();
            }
            else if (id == 4)
            {
                quest = quest.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                    .Where(x => x.LimitOfPeople > x.countPeople)
                .OrderByDescending(x => x.Id).ThenBy(x => x.Name).ToList();
            }

            return Json(quest);
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
                UserInQuestDTO userInQuestDTO = UserInQuestServices.GetAll().FirstOrDefault(x=>x.QuestId == module && x.UserId == Identity.user.Id && x.StatusId==1);
                userInQuestDTO.StatusId = 2;
                UserInQuestServices.Update(userInQuestDTO);
            }
            return RedirectToAction("InformQuest/" + module);
        }

    }
}