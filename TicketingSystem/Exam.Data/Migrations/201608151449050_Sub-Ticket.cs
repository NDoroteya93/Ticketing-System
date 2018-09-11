namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "Ticket_Id", c => c.Int());
            CreateIndex("dbo.Tickets", "Ticket_Id");
            AddForeignKey("dbo.Tickets", "Ticket_Id", "dbo.Tickets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Ticket_Id", "dbo.Tickets");
            DropIndex("dbo.Tickets", new[] { "Ticket_Id" });
            DropColumn("dbo.Tickets", "Ticket_Id");
        }
    }
}
