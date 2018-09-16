namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUnitPriceAndSubTotalColumnsToSalesProductTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesProducts", "UnitPrice", c => c.Single(nullable: false));
            AddColumn("dbo.SalesProducts", "SubTotal", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesProducts", "SubTotal");
            DropColumn("dbo.SalesProducts", "UnitPrice");
        }
    }
}
