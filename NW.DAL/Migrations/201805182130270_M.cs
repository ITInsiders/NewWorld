namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PointId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Message = c.String(),
                        GeoSuccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Points", t => t.PointId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PointId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Points",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        QuestId = c.Int(nullable: false),
                        Task = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quests", t => t.QuestId, cascadeDelete: true)
                .Index(t => t.QuestId);
            
            CreateTable(
                "dbo.Quests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        StartQuest = c.DateTime(nullable: false),
                        LimitOfPeople = c.Int(),
                        Creater = c.Int(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        SRC = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creater, cascadeDelete: true)
                .Index(t => t.Creater);
            
            CreateTable(
                "dbo.Prizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestId = c.Int(nullable: false),
                        Name = c.String(),
                        MinPlace = c.Int(nullable: false),
                        MaxPlace = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quests", t => t.QuestId, cascadeDelete: true)
                .Index(t => t.QuestId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        DateOfBirth = c.DateTime(),
                        DateOfRegistration = c.DateTime(nullable: false),
                        DateOfLastVisit = c.DateTime(),
                        DateOfLastChange = c.DateTime(),
                        Rating = c.Double(),
                        Access = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Longitude = c.Double(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Tags = c.String(),
                        WorkingHour = c.String(),
                        Description = c.String(),
                        Address = c.String(),
                        Site = c.String(),
                        Phone = c.String(),
                        Creater = c.Int(),
                        DateCreate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creater)
                .Index(t => t.Creater);
            
            CreateTable(
                "dbo.PlacePhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlaceId = c.Int(nullable: false),
                        SRC = c.String(),
                        Main = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .Index(t => t.PlaceId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        Comment = c.String(),
                        ValueLike = c.Int(nullable: false),
                        Checkin = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PlaceId);
            
            CreateTable(
                "dbo.UserInQuests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        QuestId = c.Int(),
                        ExpirationDate = c.DateTime(),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quests", t => t.QuestId)
                .ForeignKey("dbo.Statuses", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.QuestId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Statuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserPhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SRC = c.String(),
                        MainPhoto = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserVerifications",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quests", "Creater", "dbo.Users");
            DropForeignKey("dbo.UserVerifications", "Id", "dbo.Users");
            DropForeignKey("dbo.UserPhotos", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserInQuests", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserInQuests", "StatusId", "dbo.Statuses");
            DropForeignKey("dbo.UserInQuests", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Places", "Creater", "dbo.Users");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.PlacePhotos", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.Answers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Prizes", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Points", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Answers", "PointId", "dbo.Points");
            DropIndex("dbo.UserVerifications", new[] { "Id" });
            DropIndex("dbo.UserPhotos", new[] { "UserId" });
            DropIndex("dbo.UserInQuests", new[] { "StatusId" });
            DropIndex("dbo.UserInQuests", new[] { "QuestId" });
            DropIndex("dbo.UserInQuests", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "PlaceId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.PlacePhotos", new[] { "PlaceId" });
            DropIndex("dbo.Places", new[] { "Creater" });
            DropIndex("dbo.Prizes", new[] { "QuestId" });
            DropIndex("dbo.Quests", new[] { "Creater" });
            DropIndex("dbo.Points", new[] { "QuestId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "PointId" });
            DropTable("dbo.UserVerifications");
            DropTable("dbo.UserPhotos");
            DropTable("dbo.Statuses");
            DropTable("dbo.UserInQuests");
            DropTable("dbo.Reviews");
            DropTable("dbo.PlacePhotos");
            DropTable("dbo.Places");
            DropTable("dbo.Users");
            DropTable("dbo.Prizes");
            DropTable("dbo.Quests");
            DropTable("dbo.Points");
            DropTable("dbo.Answers");
        }
    }
}
