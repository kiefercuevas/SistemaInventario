namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLastNameColunmToClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "LastName", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "LastName");
        }
    }
}
