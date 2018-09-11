namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDataTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "CreateDate", c => c.DateTime());
            AlterColumn("dbo.Tickets", "EndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Tickets", "CreateDate", c => c.DateTime(nullable: false));
        }
    }
}
