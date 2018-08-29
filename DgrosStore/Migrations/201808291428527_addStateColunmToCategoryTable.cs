namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStateColunmToCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "State", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "State");
        }
    }
}
