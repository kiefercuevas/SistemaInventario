namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class restoreImageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Image", c => c.String());
            DropColumn("dbo.Products", "ImageRoute");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageRoute", c => c.String());
            DropColumn("dbo.Products", "Image");
        }
    }
}
