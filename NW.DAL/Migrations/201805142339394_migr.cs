namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migr : DbMigration
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
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
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
                "dbo.UserInQuests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        QuestId = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quests", t => t.QuestId, cascadeDelete: true)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInQuests", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserInQuests", "StatusId", "dbo.Statuses");
            DropForeignKey("dbo.UserInQuests", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Quests", "Creater", "dbo.Users");
            DropForeignKey("dbo.Answers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Prizes", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Points", "QuestId", "dbo.Quests");
            DropForeignKey("dbo.Answers", "PointId", "dbo.Points");
            DropIndex("dbo.UserInQuests", new[] { "StatusId" });
            DropIndex("dbo.UserInQuests", new[] { "QuestId" });
            DropIndex("dbo.UserInQuests", new[] { "UserId" });
            DropIndex("dbo.Prizes", new[] { "QuestId" });
            DropIndex("dbo.Quests", new[] { "Creater" });
            DropIndex("dbo.Points", new[] { "QuestId" });
            DropIndex("dbo.Answers", new[] { "UserId" });
            DropIndex("dbo.Answers", new[] { "PointId" });
            DropTable("dbo.Statuses");
            DropTable("dbo.UserInQuests");
            DropTable("dbo.Prizes");
            DropTable("dbo.Quests");
            DropTable("dbo.Points");
            DropTable("dbo.Answers");
        }
    }
}
