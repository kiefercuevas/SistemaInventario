namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiscountTableAndManyToManyRelationToProductAndDiscountAndSalesAndDiscount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        DiscountId = c.Int(nullable: false, identity: true),
                        DiscountName = c.String(),
                        DiscountType = c.String(),
                        Discountvalue = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.DiscountId);
            
            CreateTable(
                "dbo.ProductsDiscount",
                c => new
                    {
                        DiscountId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DiscountId, t.ProductId })
                .ForeignKey("dbo.Discounts", t => t.DiscountId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.DiscountId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.SalesDiscounts",
                c => new
                    {
                        SalesId = c.Int(nullable: false),
                        DiscountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SalesId, t.DiscountId })
                .ForeignKey("dbo.Sales", t => t.SalesId, cascadeDelete: true)
                .ForeignKey("dbo.Discounts", t => t.DiscountId, cascadeDelete: true)
                .Index(t => t.SalesId)
                .Index(t => t.DiscountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesDiscounts", "DiscountId", "dbo.Discounts");
            DropForeignKey("dbo.SalesDiscounts", "SalesId", "dbo.Sales");
            DropForeignKey("dbo.DiscountProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DiscountProducts", "DiscountId", "dbo.Discounts");
            DropIndex("dbo.SalesDiscounts", new[] { "DiscountId" });
            DropIndex("dbo.SalesDiscounts", new[] { "SalesId" });
            DropIndex("dbo.DiscountProducts", new[] { "ProductId" });
            DropIndex("dbo.DiscountProducts", new[] { "DiscountId" });
            DropTable("dbo.SalesDiscounts");
            DropTable("dbo.DiscountProducts");
            DropTable("dbo.Discounts");
        }
    }
}
