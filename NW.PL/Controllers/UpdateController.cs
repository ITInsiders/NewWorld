using System;
using System.IO;
using System.Text.RegularExpressions;
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
    public class UpdateController : Controller
    {
        PageInfo pageInfo = PageInfo.Create("Update");
        Identity Identity = new Identity();
        CryptMD5 cryptMD5 = new CryptMD5();

        public ActionResult Place()
        {
            ViewBag.Page = pageInfo.setView("Place");
            return View();
        }

        public ActionResult Quest(string id)
        {
            ViewBag.Page = pageInfo.setView("Quest");
            ViewBag.Message = TempData["Message"];
            if (id != null)
            {
                int ID = Convert.ToInt32(id);
                QuestDTO questDTO = QuestServices.Get(ID);

                return View(questDTO);
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddPlace(HttpPostedFileBase[] Photo)
        {
            string Name = Request.Form["Name"];
            string Address = Request.Form["Address"];
            string Site = Request.Form["Site"];
            string Phone = Request.Form["Phone"];
            string Description = Request.Form["Description"];
            string Coordinates = Request.Form["Coordinates"];
            var re = new Regex(@"[\[\]]");
            double[] NewCoordinates = re.Replace(Coordinates, "").Split(',')
                .Select(x => Convert.ToDouble(x.Replace(".", ","))).ToArray();
            string Tags = Request.Form["Tags"];
            DateTime DateCreate = DateTime.Now;


            if (Name != "" && Address != "" && NewCoordinates.Length == 2 && Identity.isAuthentication)
            {
                PlaceDTO placeDTO = new PlaceDTO();
                placeDTO.Address = Address;
                placeDTO.Creater = Identity.user.Id;
                placeDTO.Longitude = NewCoordinates[0];
                placeDTO.Latitude = NewCoordinates[1];
                placeDTO.DateCreate = DateCreate;
                placeDTO.Description = Description;
                placeDTO.Name = Name;
                placeDTO.Tags = Tags; ;
                placeDTO.Site = Site;
                placeDTO.Phone = Phone;
                PlaceService.Create(placeDTO);
                PlaceDTO newPlace = PlaceService.GetAll().FirstOrDefault(x => x.Name == Name && x.Creater == Identity.user.Id && x.DateCreate == DateCreate);
                PlacePhotoDTO placePhoto;
                string dir = Server.MapPath("~/Resources/Images/Places/" + newPlace.Id);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (Photo.Count() > 0)
                {
                    foreach (HttpPostedFileBase photo in Photo)
                    {
                        string type = photo.FileName.Split('.').Last();

                        string[] dirs = Directory.GetFiles(Server.MapPath("~/Resources/Images/Places/" + newPlace.Id), "*");
                        string src = "/Resources/Images/Places/" + newPlace.Id + "/" + cryptMD5.GetHash(dirs.Length.ToString()) + "." + type;
                        string path = Server.MapPath("~" + src);

                        photo.SaveAs(path);

                        placePhoto = new PlacePhotoDTO();
                        placePhoto.Main = false;
                        placePhoto.SRC = src;
                        placePhoto.PlaceId = newPlace.Id;
                        PlacePhotoServices.Create(placePhoto);
                    }
                    placePhoto = PlacePhotoServices.GetAll().FirstOrDefault(x => x.Main == false && x.PlaceId == newPlace.Id);
                    if (placePhoto != null)
                    {
                        placePhoto.Main = true;
                        PlacePhotoServices.Update(placePhoto);
                    }

                }
            }
            return RedirectToAction("Place");
        }

        [HttpPost]
        public ActionResult AddQuest(UpdateQuest quest)
        {
            DateTime DateCreate = DateTime.Now;

            if (quest.Points.Count > 2 && quest.Name != null && quest.Prizes.Count > 0 && Identity.isAuthentication || true)
            {
                if (quest.Id != null)
                {
                    int ID = quest.Id.Value;
                    QuestDTO questDTO = QuestServices.Get(ID);
                    questDTO.StartQuest = quest.DateTime;
                    questDTO.Description = quest.Description;
                    questDTO.LimitOfPeople = Convert.ToInt32(quest.MaxPeople);
                    questDTO.Name = quest.Name;
                    QuestServices.Update(questDTO);

                    if (quest.Photo != null)
                    {
                        string dir = Server.MapPath("~/Resources/Images/Quest");
                        string type = quest.Photo.FileName.Split('.').Last();

                        string src = "/Resources/Images/Quest/" + cryptMD5.GetHash(Convert.ToString(questDTO.Id)) + "." + type;
                        string path = Server.MapPath("~" + src);

                        quest.Photo.SaveAs(path);
                        questDTO.SRC = src;
                        QuestServices.Update(questDTO);
                    }

                    List<PrizeDTO> prizeDTO = PrizeServices.GetAll().Where(x=> x.QuestId == ID).ToList();
                    List<PointDTO> pointDTO = PointServices.GetAll().Where(x => x.QuestId == ID).ToList();
                    foreach(PrizeDTO prz in prizeDTO)
                    {
                        PrizeServices.Delete(prz.Id);
                    }
                    foreach (PointDTO pnt in pointDTO)
                    {
                        PointServices.Delete(pnt.Id);
                    }
                    PrizeDTO prizeDTOnew;
                    PointDTO pointDTOnew;
                    foreach (UpdateQuest.Prize prize in quest.Prizes)
                    {
                        prizeDTOnew = new PrizeDTO();
                        prizeDTOnew.MaxPlace = prize.Max;
                        prizeDTOnew.MinPlace = prize.Min;
                        prizeDTOnew.Name = prize.Title;
                        prizeDTOnew.QuestId = ID;
                        PrizeServices.Create(prizeDTOnew);
                    }
                    foreach (UpdateQuest.Point point in quest.Points)
                    {
                        pointDTOnew = new PointDTO();
                        pointDTOnew.Latitude = point.getLatitude();
                        pointDTOnew.Longitude = point.getLongitude();
                        pointDTOnew.Task = point.Task;
                        pointDTOnew.QuestId = ID;
                        pointDTOnew.Address = point.Address;
                        PointServices.Create(pointDTOnew);
                    }
                    TempData["Message"] = "Успешно отредактировано";
                    return Redirect("/Home/Home");

                }
                else
                {
                    QuestDTO questDTO = new QuestDTO();
                    questDTO.StartQuest = quest.DateTime;
                    questDTO.DateCreate = DateCreate;
                    questDTO.Description = quest.Description;
                    questDTO.LimitOfPeople = Convert.ToInt32(quest.MaxPeople);
                    questDTO.Name = quest.Name;
                    questDTO.Creater = Identity.user.Id;
                    QuestServices.Create(questDTO);
                    QuestDTO newQuestDTO = QuestServices.GetAll().FirstOrDefault(x => x.Name == quest.Name && x.Creater == Identity.user.Id && x.DateCreate == DateCreate);
                    if (quest.Photo != null)
                    {
                        string dir = Server.MapPath("~/Resources/Images/Quest");
                        string type = quest.Photo.FileName.Split('.').Last();

                        string src = "/Resources/Images/Quest/" + cryptMD5.GetHash(Convert.ToString(newQuestDTO.Id)) + "." + type;
                        string path = Server.MapPath("~" + src);

                        quest.Photo.SaveAs(path);
                        newQuestDTO.SRC = src;
                        QuestServices.Update(newQuestDTO);
                    }


                    PrizeDTO prizeDTO;
                    PointDTO pointDTO;
                    foreach (UpdateQuest.Prize prize in quest.Prizes)
                    {
                        prizeDTO = new PrizeDTO();
                        prizeDTO.MaxPlace = prize.Max;
                        prizeDTO.MinPlace = prize.Min;
                        prizeDTO.Name = prize.Title;
                        prizeDTO.QuestId = newQuestDTO.Id;
                        PrizeServices.Create(prizeDTO);
                    }
                    foreach (UpdateQuest.Point point in quest.Points)
                    {
                        pointDTO = new PointDTO();
                        pointDTO.Latitude = point.getLatitude();
                        pointDTO.Longitude = point.getLongitude();
                        pointDTO.Task = point.Task;
                        pointDTO.QuestId = newQuestDTO.Id;
                        pointDTO.Address = point.Address;
                        PointServices.Create(pointDTO);
                    }
                    TempData["Message"] = "Успешно добавлено";
                    
                    return Redirect("/Home/Home");
                }

            }
            TempData["Message"] = "Квест не добавлен";
            return RedirectToAction("Quest");
        }

        public ActionResult DeleteQuest(int id)
        {
            if (Identity.isAuthentication)
            {
                QuestServices.Delete(id);
            }
            return RedirectToAction("/Home/Home");
        }
    }
}