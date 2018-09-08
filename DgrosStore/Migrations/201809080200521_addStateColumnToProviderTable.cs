namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStateColumnToProviderTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Providers", "State", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Providers", "State");
        }
    }
}
