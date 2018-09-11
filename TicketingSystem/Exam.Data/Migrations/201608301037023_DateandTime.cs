namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateandTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tickets", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "EndDate");
            DropColumn("dbo.Tickets", "CreateDate");
        }
    }
}
