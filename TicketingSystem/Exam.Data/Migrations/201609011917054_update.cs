namespace TicketingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Name", c => c.String(maxLength: 50));
            AlterColumn("dbo.Tickets", "Title", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "Title", c => c.String());
            AlterColumn("dbo.Categories", "Name", c => c.String());
        }
    }
}
