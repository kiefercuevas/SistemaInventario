namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeRelationIntoClientAndSalesFromOneToManyToManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Sales", "ClientId", "dbo.Clients");
            DropIndex("dbo.Sales", new[] { "ClientId" });
            CreateTable(
                "dbo.clientSales",
                c => new
                    {
                        salesId = c.Int(nullable: false),
                        clientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.salesId, t.clientId })
                .ForeignKey("dbo.Sales", t => t.salesId, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.clientId, cascadeDelete: true)
                .Index(t => t.salesId)
                .Index(t => t.clientId);
            
            DropColumn("dbo.Sales", "ClientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "ClientId", c => c.Int(nullable: false));
            DropForeignKey("dbo.clientSales", "clientId", "dbo.Clients");
            DropForeignKey("dbo.clientSales", "salesId", "dbo.Sales");
            DropIndex("dbo.clientSales", new[] { "clientId" });
            DropIndex("dbo.clientSales", new[] { "salesId" });
            DropTable("dbo.clientSales");
            CreateIndex("dbo.Sales", "ClientId");
            AddForeignKey("dbo.Sales", "ClientId", "dbo.Clients", "ClientId");
        }
    }
}
