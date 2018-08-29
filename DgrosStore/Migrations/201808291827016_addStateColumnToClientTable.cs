namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStateColumnToClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "State", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "State");
        }
    }
}
