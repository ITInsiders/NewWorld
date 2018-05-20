using System.Data.Entity;

using NW.DAL.Entities;

namespace NW.DAL.EF
{
    public class Context : DbContext
    {
        public Context() : base("NewWorldDB") { }

        public DbSet<User> Users { get; }
        public DbSet<UserPhoto> UserPhotos { get; }
        public DbSet<UserVerification> UserVerifications { get; }
        public DbSet<Place> Places { get; }
        public DbSet<PlacePhoto> PlacePhotos { get; }
        public DbSet<Review> Reviews { get; }
        public DbSet<Quest> Quests { get; }
        public DbSet<Status> Statuses { get; }
        public DbSet<UserInQuest> UserInQuests { get; }
        public DbSet<Prize> Prizes { get; }
        public DbSet<Point> Points { get; }
        public DbSet<Answer> Answers { get; }

    }
}
