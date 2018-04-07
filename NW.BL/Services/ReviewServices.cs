using System.Collections.Generic;
using System.Linq;

using NW.BL.DTO;
using NW.BL.Extensions;
using NW.DAL.Entities;
using NW.DAL.Repositories;

namespace NW.BL.Services
{
    public class ReviewServices
    {
        private static EFUnitOfWork Database = new EFUnitOfWork();

        public static void Create(ReviewDTO reviewDTO)
        {
            Review review = MapperTransform<Review, ReviewDTO>.ToEntity(reviewDTO);
            Database.Reviews.Create(review);
            Database.Save();
        }
        public static List<ReviewDTO> GetAll()
        {
            List<Review> review = Database.Reviews.GetAll().ToList();
            return MapperTransform<Review, ReviewDTO>.ToModelCollection(review);
        }
        public static ReviewDTO Get(int id)
        {
            Review review = Database.Reviews.Get(id);
            return MapperTransform<Review, ReviewDTO>.ToModel(review);
        }

        public static void Update(ReviewDTO reviewDTO)
        {
            Review review = Database.Reviews.Get(reviewDTO.Id);
            review.UserId = reviewDTO.UserId;
            review.PlaceId = reviewDTO.PlaceId;
            review.Comment = reviewDTO.Comment;
            review.ValueLike = reviewDTO.ValueLike;
            review.Checkin = reviewDTO.Checkin;
            Database.Reviews.Update(review);
            Database.Save();
        }
        public static void Delete(int id)
        {
            Database.Reviews.Delete(id);
            Database.Save();
        }
    }
}
