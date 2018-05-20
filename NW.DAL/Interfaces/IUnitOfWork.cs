using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NW.DAL.Entities;

namespace NW.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Review> Reviews { get; }
        IRepository<Place> Places { get; }
        IRepository<PlacePhoto> PlacePhotos { get; }
        IRepository<User> Users { get; }
        IRepository<UserPhoto> UserPhotos { get; }
        IRepository<UserVerification> UserVerifications { get; }
        IRepository<Quest> Quests { get; }
        IRepository<Status> Statuses { get; }
        IRepository<UserInQuest> UserInQuests { get; }
        IRepository<Prize> Prizes { get; }
        IRepository<Point> Points { get; }
        IRepository<Answer> Answers { get; }
        void Save();
    }
}
