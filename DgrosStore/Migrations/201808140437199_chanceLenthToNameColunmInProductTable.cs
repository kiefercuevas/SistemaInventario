namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chanceLenthToNameColunmInProductTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false));
        }
    }
}
