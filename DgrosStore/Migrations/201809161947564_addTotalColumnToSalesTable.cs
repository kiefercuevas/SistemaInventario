namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTotalColumnToSalesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sales", "Total", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sales", "Total");
        }
    }
}
