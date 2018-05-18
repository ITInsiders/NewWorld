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
        public ActionResult Quest()
        {
            ViewBag.Page = pageInfo.setView("Quest");
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
                    foreach(HttpPostedFileBase photo in Photo)
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


            //if (Name != "" && Address != "" && NewCoordinates.Length == 2 && Identity.isAuthentication)
            //{
            //    PlaceDTO placeDTO = new PlaceDTO();
            //    placeDTO.Address = Address;
            //    placeDTO.Creater = Identity.user.Id;
            //    placeDTO.Longitude = NewCoordinates[0];
            //    placeDTO.Latitude = NewCoordinates[1];
            //    placeDTO.DateCreate = DateCreate;
            //    placeDTO.Description = Description;
            //    placeDTO.Name = Name;
            //    placeDTO.Tags = Tags; ;
            //    placeDTO.Site = Site;
            //    placeDTO.Phone = Phone;
            //    PlaceService.Create(placeDTO);
            //    PlaceDTO newPlace = PlaceService.GetAll().FirstOrDefault(x => x.Name == Name && x.Creater == Identity.user.Id && x.DateCreate == DateCreate);
            //    PlacePhotoDTO placePhoto;
            //    string dir = Server.MapPath("~/Resources/Images/Places/" + newPlace.Id);
            //    if (!Directory.Exists(dir))
            //        Directory.CreateDirectory(dir);
            //    if (Photo.Count() > 0)
            //    {
            //        foreach (HttpPostedFileBase photo in Photo)
            //        {
            //            string type = photo.FileName.Split('.').Last();

            //            string[] dirs = Directory.GetFiles(Server.MapPath("~/Resources/Images/Places/" + newPlace.Id), "*");
            //            string src = "/Resources/Images/Places/" + newPlace.Id + "/" + cryptMD5.GetHash(dirs.Length.ToString()) + "." + type;
            //            string path = Server.MapPath("~" + src);

            //            photo.SaveAs(path);

            //            placePhoto = new PlacePhotoDTO();
            //            placePhoto.Main = false;
            //            placePhoto.SRC = src;
            //            placePhoto.PlaceId = newPlace.Id;
            //            PlacePhotoServices.Create(placePhoto);
            //        }
            //        placePhoto = PlacePhotoServices.GetAll().FirstOrDefault(x => x.Main == false && x.PlaceId == newPlace.Id);
            //        if (placePhoto != null)
            //        {
            //            placePhoto.Main = true;
            //            PlacePhotoServices.Update(placePhoto);
            //        }

            //    }
            //}
            return RedirectToAction("Quest");
        }
    }
}