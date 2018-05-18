namespace NW.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quests", "SRC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Quests", "SRC");
        }
    }
}
