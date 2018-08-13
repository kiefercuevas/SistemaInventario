namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ShoppingPrice = c.Single(nullable: false),
                        Stock = c.Int(nullable: false),
                        Description = c.String(),
                        State = c.Boolean(nullable: false),
                        Image = c.String(),
                        Slug = c.String(nullable: false, maxLength: 255),
                        SellingPrice = c.Single(nullable: false),
                        MinimunStock = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        StoreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Store", t => t.StoreId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.StoreId);
            
            CreateTable(
                "dbo.SalesProducts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        SalesId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.SalesId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.SalesId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.SalesId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        ClientId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        paymentMethod = c.Int(nullable: false),
                        State = c.Boolean(nullable: false),
                        Commentary = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Direcction = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Telephones",
                c => new
                    {
                        TelephoneId = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        CliendId = c.Int(nullable: false),
                        Client_ClientId = c.Int(),
                    })
                .PrimaryKey(t => t.TelephoneId)
                .ForeignKey("dbo.Clients", t => t.Client_ClientId)
                .Index(t => t.Client_ClientId);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        StoreId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Address = c.String(),
                        Telephone = c.String(nullable: false),
                        Logo = c.String(),
                        Slogan = c.String(),
                    })
                .PrimaryKey(t => t.StoreId);
            
            CreateTable(
                "dbo.Shopping",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StoreId = c.Int(nullable: false),
                        ProviderId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        paymentMethod = c.Int(nullable: false),
                        State = c.Boolean(nullable: false),
                        Commentary = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Providers", t => t.ProviderId)
                .ForeignKey("dbo.Store", t => t.StoreId)
                .Index(t => t.StoreId)
                .Index(t => t.ProviderId);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        ProviderId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Direcction = c.String(),
                        Email = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.ProviderId);
            
            CreateTable(
                "dbo.ShoppingProducts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        ShoppingId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.ShoppingId })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Shopping", t => t.ShoppingId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.ShoppingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Sales", "StoreId", "dbo.Store");
            DropForeignKey("dbo.Shopping", "StoreId", "dbo.Store");
            DropForeignKey("dbo.ShoppingProducts", "ShoppingId", "dbo.Shopping");
            DropForeignKey("dbo.ShoppingProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Shopping", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.SalesProducts", "SalesId", "dbo.Sales");
            DropForeignKey("dbo.Sales", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Telephones", "Client_ClientId", "dbo.Clients");
            DropForeignKey("dbo.SalesProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.ShoppingProducts", new[] { "ShoppingId" });
            DropIndex("dbo.ShoppingProducts", new[] { "ProductId" });
            DropIndex("dbo.Shopping", new[] { "ProviderId" });
            DropIndex("dbo.Shopping", new[] { "StoreId" });
            DropIndex("dbo.Telephones", new[] { "Client_ClientId" });
            DropIndex("dbo.Sales", new[] { "ClientId" });
            DropIndex("dbo.Sales", new[] { "StoreId" });
            DropIndex("dbo.SalesProducts", new[] { "SalesId" });
            DropIndex("dbo.SalesProducts", new[] { "ProductId" });
            DropIndex("dbo.Products", new[] { "StoreId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropTable("dbo.ShoppingProducts");
            DropTable("dbo.Providers");
            DropTable("dbo.Shopping");
            DropTable("dbo.Store");
            DropTable("dbo.Telephones");
            DropTable("dbo.Clients");
            DropTable("dbo.Sales");
            DropTable("dbo.SalesProducts");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
