namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeUniquePropertiesByDataAnotationToProductAndCategory : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", new[] { "Name" });
            DropIndex("dbo.Products", new[] { "Name" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Products", "Name", unique: true);
            CreateIndex("dbo.Categories", "Name", unique: true);
        }
    }
}
