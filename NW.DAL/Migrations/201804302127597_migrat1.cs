namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrat1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "Date");
        }
    }
}
