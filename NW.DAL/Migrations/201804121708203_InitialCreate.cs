namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        PlaceId = c.Int(nullable: false),
                        Comment = c.String(),
                        ValueLike = c.Int(nullable: false),
                        Checkin = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Places", t => t.PlaceId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PlaceId);
            
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
                "dbo.UserPhotoes",
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
            DropForeignKey("dbo.UserVerifications", "Id", "dbo.Users");
            DropForeignKey("dbo.UserPhotoes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Places", "Creater", "dbo.Users");
            DropForeignKey("dbo.Reviews", "PlaceId", "dbo.Places");
            DropForeignKey("dbo.PlacePhotos", "PlaceId", "dbo.Places");
            DropIndex("dbo.UserVerifications", new[] { "Id" });
            DropIndex("dbo.UserPhotoes", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "PlaceId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Places", new[] { "Creater" });
            DropIndex("dbo.PlacePhotos", new[] { "PlaceId" });
            DropTable("dbo.UserVerifications");
            DropTable("dbo.UserPhotoes");
            DropTable("dbo.Users");
            DropTable("dbo.Reviews");
            DropTable("dbo.Places");
            DropTable("dbo.PlacePhotos");
        }
    }
}
