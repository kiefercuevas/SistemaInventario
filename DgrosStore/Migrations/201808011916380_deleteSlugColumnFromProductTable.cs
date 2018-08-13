namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteSlugColumnFromProductTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Slug");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Slug", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
