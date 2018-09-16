namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addValidationToDiscountTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Discounts", "DiscountName", c => c.String(nullable: false));
            AlterColumn("dbo.Discounts", "DiscountType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Discounts", "DiscountType", c => c.String());
            AlterColumn("dbo.Discounts", "DiscountName", c => c.String());
        }
    }
}
