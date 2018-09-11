namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateParetnField1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tickets", "ParentTicketId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tickets", "ParentTicketId", c => c.Int(nullable: false));
        }
    }
}
