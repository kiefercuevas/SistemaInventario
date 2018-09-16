namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTotalColumnTypeInSalesTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sales", "Total", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sales", "Total", c => c.Int(nullable: false));
        }
    }
}
