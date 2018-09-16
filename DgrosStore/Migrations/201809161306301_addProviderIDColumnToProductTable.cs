namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProviderIDColumnToProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProviderId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "ProviderId");
            AddForeignKey("dbo.Products", "ProviderId", "dbo.Providers", "PersonId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProviderId", "dbo.Providers");
            DropIndex("dbo.Products", new[] { "ProviderId" });
            DropColumn("dbo.Products", "ProviderId");
        }
    }
}
