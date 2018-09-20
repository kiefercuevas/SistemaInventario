namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeImageColumnToImageRouteInProductsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImageRoute", c => c.String());
            DropColumn("dbo.Products", "Image");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Image", c => c.String());
            DropColumn("dbo.Products", "ImageRoute");
        }
    }
}
