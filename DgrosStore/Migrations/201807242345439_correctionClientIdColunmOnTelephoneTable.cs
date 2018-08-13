namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctionClientIdColunmOnTelephoneTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Telephones", "Client_ClientId", "dbo.Clients");
            DropIndex("dbo.Telephones", new[] { "Client_ClientId" });
            RenameColumn(table: "dbo.Telephones", name: "Client_ClientId", newName: "ClientId");
            AlterColumn("dbo.Telephones", "ClientId", c => c.Int(nullable: false));
            CreateIndex("dbo.Telephones", "ClientId");
            AddForeignKey("dbo.Telephones", "ClientId", "dbo.Clients", "ClientId", cascadeDelete: true);
            DropColumn("dbo.Telephones", "CliendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Telephones", "CliendId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Telephones", "ClientId", "dbo.Clients");
            DropIndex("dbo.Telephones", new[] { "ClientId" });
            AlterColumn("dbo.Telephones", "ClientId", c => c.Int());
            RenameColumn(table: "dbo.Telephones", name: "ClientId", newName: "Client_ClientId");
            CreateIndex("dbo.Telephones", "Client_ClientId");
            AddForeignKey("dbo.Telephones", "Client_ClientId", "dbo.Clients", "ClientId");
        }
    }
}
