using System;

using NW.DAL.EF;
using NW.DAL.Interfaces;
using NW.DAL.Entities;

namespace NW.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private static EFUnitOfWork instance;
        public static EFUnitOfWork I => instance ?? (instance = new EFUnitOfWork());

        private Context db;
        private Repository<Place> placeRepository;
        private Repository<PlacePhoto> placePhotoRepository;
        private Repository<Review> reviewRepository;
        private Repository<User> userRepository;
        private Repository<UserPhoto> userPhotoRepository;
        private Repository<UserVerification> userVerificationRepository;
        private Repository<Quest> questRepository;
        private Repository<Status> statusRepository;
        private Repository<UserInQuest> userInQuestRepository;
        private Repository<Prize> prizeRepository;
        private Repository<Point> pointRepository;
        private Repository<Answer> answerRepository;

        public EFUnitOfWork()
        {
            db = new Context();
        }

        public IRepository<Place> Places
        {
            get => placeRepository ?? (placeRepository = new Repository<Place>(db));
        }

        public IRepository<PlacePhoto> PlacePhotos
        {
            get => placePhotoRepository ?? (placePhotoRepository = new Repository<PlacePhoto>(db));
        }

        public IRepository<Review> Reviews
        {
            get => reviewRepository ?? (reviewRepository = new Repository<Review>(db));
        }

        public IRepository<User> Users
        {
            get => userRepository ?? (userRepository = new Repository<User>(db));
        }

        public IRepository<UserPhoto> UserPhotos
        {
            get => userPhotoRepository ?? (userPhotoRepository = new Repository<UserPhoto>(db));
        }

        public IRepository<UserVerification> UserVerifications
        {
            get => userVerificationRepository ?? (userVerificationRepository = new Repository<UserVerification>(db));
        }
        public IRepository<Quest> Quests
        {
            get => questRepository ?? (questRepository = new Repository<Quest>(db));
        }
        public IRepository<Status> Statuses
        {
            get => statusRepository ?? (statusRepository = new Repository<Status>(db));
        }
        public IRepository<UserInQuest> UserInQuests
        {
            get => userInQuestRepository ?? (userInQuestRepository = new Repository<UserInQuest>(db));
        }
        public IRepository<Prize> Prizes
        {
            get => prizeRepository ?? (prizeRepository = new Repository<Prize>(db));
        }
        public IRepository<Point> Points
        {
            get => pointRepository ?? (pointRepository = new Repository<Point>(db));
        }
        public IRepository<Answer> Answers
        {
            get => answerRepository ?? (answerRepository = new Repository<Answer>(db));
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.SaveChanges();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
