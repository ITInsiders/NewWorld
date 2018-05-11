namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class M : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserPhotoes", newName: "UserPhotos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserPhotos", newName: "UserPhotoes");
        }
    }
}
