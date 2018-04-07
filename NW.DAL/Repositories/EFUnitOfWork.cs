using System;

using NW.DAL.EF;
using NW.DAL.Interfaces;
using NW.DAL.Entities;

namespace NW.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private Context db;
        private Repository<Place> placeRepository;
        private Repository<PlacePhoto> placePhotoRepository;
        private Repository<Review> reviewRepository;
        private Repository<User> userRepository;
        private Repository<UserPhoto> userPhotoRepository;
        private Repository<UserVerification> userVerificationRepository;

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
