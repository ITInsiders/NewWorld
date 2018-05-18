namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrati : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInQuests", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInQuests", "ExpirationDate", c => c.DateTime(nullable: false));
        }
    }
}
