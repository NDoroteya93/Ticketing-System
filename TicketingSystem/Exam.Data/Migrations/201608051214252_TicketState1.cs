namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketState1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "TaskState", c => c.Int(nullable: false));
            DropColumn("dbo.Tickets", "TastState");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "TastState", c => c.Int(nullable: false));
            DropColumn("dbo.Tickets", "TaskState");
        }
    }
}
