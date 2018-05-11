using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NW.BL.Services;
using NW.BL.DTO;
using NW.PL.Models;

namespace NW.PL.Controllers
{
    public class MapController : Controller
    {
        PageInfo pageInfo = PageInfo.Create("Map");
        Identity Identity = new Identity();

        public ActionResult Search()
        {
            ViewBag.Page = pageInfo.setView("Search");
            return View();
        }

        public JsonResult SearchLines()
        {
            string Search = Request.Form["Search"];

            List<PlaceDTO> places = PlaceService.GetAll();
            List<SearchLine> blocks = new List<SearchLine>();

            blocks.AddRange(places.Where(x => x.Name.ToLower().Contains(Search.ToLower()))
                .OrderByDescending(x => x.Rating).ThenBy(x => x.Name)
                .Select(x => new SearchLine() { Type = x.Tags, Value = x.Name }));

            blocks.AddRange(places.Where(x => x.Tags.ToLower().Contains(Search.ToLower()))
                .OrderBy(x => x.Tags).Select(x => new SearchLine() { Type = "Тег", Value = x.Tags }));

            return Json(blocks);
        }

        public JsonResult SearchBlocks()
        {
            string Search = Request.Form["Search"];

            List<PlaceDTO> places = PlaceService.GetAll()
                .Where(x => x.Name.ToLower().Contains(Search.ToLower()) || x.Tags.ToLower().Contains(Search.ToLower()))
                .OrderByDescending(x => x.Rating).ThenBy(x => x.Name).ToList();

            return Json(places);
        }

        //----------------------------------------------------------------------------------

        public ActionResult InformPlace(int id)
        {
            ViewBag.Page = pageInfo.setView("InformPlace").setTitle("InformPlace");
            ViewBag.Method = HttpContext.Request.HttpMethod;

            PlaceDTO placeDTO = PlaceService.Get(id);
            if (placeDTO == null) return HttpNotFound();
            List<PlacePhotoDTO> photo = PlacePhotoServices.GetAll().Where(x => x.PlaceId == id).ToList();
            List<ReviewDTO> review = ReviewServices.GetAll().Where(x => x.PlaceId == id).OrderByDescending(x => x.Date).ToList();

            //IQueryable<Rating> R = db.Ratings.Where(x => x.IdPlace == id);
            
             int RLike = review.Count(x => x.ValueLike == 1);
             int RDis = review.Count(x => x.ValueLike == 2);
             int RCheck = review.Count(x => x.Checkin == 1);
             int RRating = (RLike + RDis == 0) ? 0 : Convert.ToInt32(Math.Round(10.0 * Convert.ToDouble(RLike) / Convert.ToDouble(RLike + RDis)));

            List<UserDTO> userDTO = UserServices.GetAll();
            
            
            PlaceDTO obj = new PlaceDTO()
            {
                Id = placeDTO.Id,
                Name = placeDTO.Name,
                Longitude = placeDTO.Longitude,
                Latitude = placeDTO.Latitude,
                Address = placeDTO.Address,
                Description = placeDTO.Description,
                Phone = placeDTO.Phone,
                
                Site = placeDTO.Site,
                WorkingHour = placeDTO.WorkingHour
            };
            ViewBag.Album = photo;
            ViewBag.Checkins = RCheck;
            ViewBag.Comments = review;
            ViewBag.Dislike = RDis;
            ViewBag.Like = RLike;
            ViewBag.Ratings = RRating;
            ViewBag.Login = userDTO;
            return View(obj);
        }

        //--------------------------------------------------------------
        public ActionResult AddComment(string Text, string Id)
        {
            if (Identity.isAuthentication)
            {
                ReviewDTO reviewDTO = new ReviewDTO
                {
                    PlaceId = Convert.ToInt32(Id),
                    UserId = Identity.user.Id,
                    Comment = Text,
                    Date = DateTime.Now
                };

                ReviewServices.Create(reviewDTO);
            }
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }

        public JsonResult AddRatingJson(int R, int IdPlace)
        {
            if (Identity.isAuthentication)
            {
                ReviewDTO reviewDTO = ReviewServices.GetAll().FirstOrDefault(x => x.PlaceId == IdPlace && x.UserId == Identity.user.Id);
                if (reviewDTO != null)
                {
                    if (R == 1 && reviewDTO.ValueLike == 1) return Json(null, JsonRequestBehavior.AllowGet);
                    else if (R == 1) reviewDTO.ValueLike = 1;

                    if (R == 2 && reviewDTO.ValueLike == 2) return Json(null, JsonRequestBehavior.AllowGet);
                    else if (R == 2) reviewDTO.ValueLike = 2;

                    if (R == 3 && reviewDTO.Checkin == 1) return Json(null, JsonRequestBehavior.AllowGet);
                    else if (R == 3) reviewDTO.Checkin = 1;

                    ReviewServices.Update(reviewDTO);
                }
                else
                {
                    ReviewDTO review = new ReviewDTO();
                    {
                        review.UserId = Identity.user.Id;
                        review.PlaceId = IdPlace;
                        review.ValueLike = (R != 3) ? R : 0;
                        review.Checkin = (R == 3) ? 1 : 0;
                        review.Date = DateTime.Now;
                    }
                    ReviewServices.Create(review);
                }
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestRatingJson(int R, int IdPlace)
        {
            if (Identity.isAuthentication)
            {
                ReviewDTO reviewDTO = ReviewServices.GetAll().FirstOrDefault(x => x.PlaceId == IdPlace && x.UserId == Identity.user.Id);
                if (reviewDTO != null)
                {
                    if (R == 2 && reviewDTO.ValueLike == 2) return Json(true, JsonRequestBehavior.AllowGet);
                    else if (R == 1 && reviewDTO.ValueLike == 1) return Json(true, JsonRequestBehavior.AllowGet);
                    else if (R == 3 && reviewDTO.Checkin == 1) return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountRatingJson(int R, int IdPlace)
        {
            List<ReviewDTO> review = ReviewServices.GetAll().Where(x => x.PlaceId == IdPlace).ToList();
            int RLike = review.Count(x => x.ValueLike == 2);
            int RDis = review.Count(x => x.ValueLike == 1);
            int RCheck = review.Count(x => x.Checkin == 1);
            int RRating = (RLike + RDis == 0) ? 0 : Convert.ToInt32(Math.Round(10.0 * Convert.ToDouble(RLike) / Convert.ToDouble(RLike + RDis)));


            if (R == 2) return Json(RLike, JsonRequestBehavior.AllowGet);
            else if (R == 1) return Json(RDis, JsonRequestBehavior.AllowGet);
            else if (R == 3) return Json(RCheck, JsonRequestBehavior.AllowGet);
            else if (R == 4) return Json(RRating, JsonRequestBehavior.AllowGet);
            else return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}