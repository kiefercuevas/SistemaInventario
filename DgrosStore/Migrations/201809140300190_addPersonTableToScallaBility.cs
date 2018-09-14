namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPersonTableToScallaBility : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Telephones", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.clientSales", "clientId", "dbo.Clients");
            DropForeignKey("dbo.Shopping", "ProviderId", "dbo.Providers");
            RenameColumn(table: "dbo.Telephones", name: "ClientId", newName: "PersonId");
            RenameIndex(table: "dbo.Telephones", name: "IX_ClientId", newName: "IX_PersonId");
            DropPrimaryKey("dbo.Clients");
            DropPrimaryKey("dbo.Providers");
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Direcction = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false),
                        Image = c.String(),
                        State = c.Boolean(nullable: false),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId);
            
            AddColumn("dbo.Clients", "PersonId", c => c.Int(nullable: false));
            AddColumn("dbo.Providers", "PersonId", c => c.Int(nullable: false));
            AddColumn("dbo.Providers", "Rnc", c => c.String());
            AlterColumn("dbo.Clients", "ClientId", c => c.Int(nullable: false));
            AlterColumn("dbo.Providers", "ProviderId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Clients", "PersonId");
            AddPrimaryKey("dbo.Providers", "PersonId");
            CreateIndex("dbo.Clients", "PersonId");
            CreateIndex("dbo.Providers", "PersonId");
            AddForeignKey("dbo.Clients", "PersonId", "dbo.People", "PersonId");
            AddForeignKey("dbo.Providers", "PersonId", "dbo.People", "PersonId");
            AddForeignKey("dbo.clientSales", "clientId", "dbo.Clients", "PersonId", cascadeDelete: true);
            AddForeignKey("dbo.Shopping", "ProviderId", "dbo.Providers", "PersonId");
            DropColumn("dbo.Clients", "Name");
            DropColumn("dbo.Clients", "Direcction");
            DropColumn("dbo.Clients", "Email");
            DropColumn("dbo.Clients", "Image");
            DropColumn("dbo.Clients", "State");
            DropColumn("dbo.Providers", "Name");
            DropColumn("dbo.Providers", "Direcction");
            DropColumn("dbo.Providers", "Email");
            DropColumn("dbo.Providers", "Telephone");
            DropColumn("dbo.Providers", "State");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Providers", "State", c => c.Boolean(nullable: false));
            AddColumn("dbo.Providers", "Telephone", c => c.String(nullable: false));
            AddColumn("dbo.Providers", "Email", c => c.String());
            AddColumn("dbo.Providers", "Direcction", c => c.String());
            AddColumn("dbo.Providers", "Name", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Clients", "State", c => c.Boolean(nullable: false));
            AddColumn("dbo.Clients", "Image", c => c.String());
            AddColumn("dbo.Clients", "Email", c => c.String());
            AddColumn("dbo.Clients", "Direcction", c => c.String());
            AddColumn("dbo.Clients", "Name", c => c.String(nullable: false, maxLength: 20));
            DropForeignKey("dbo.Shopping", "ProviderId", "dbo.Providers");
            DropForeignKey("dbo.clientSales", "clientId", "dbo.Clients");
            DropForeignKey("dbo.Providers", "PersonId", "dbo.People");
            DropForeignKey("dbo.Clients", "PersonId", "dbo.People");
            DropIndex("dbo.Providers", new[] { "PersonId" });
            DropIndex("dbo.Clients", new[] { "PersonId" });
            DropPrimaryKey("dbo.Providers");
            DropPrimaryKey("dbo.Clients");
            AlterColumn("dbo.Providers", "ProviderId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Clients", "ClientId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Providers", "Rnc");
            DropColumn("dbo.Providers", "PersonId");
            DropColumn("dbo.Clients", "PersonId");
            DropTable("dbo.People");
            AddPrimaryKey("dbo.Providers", "ProviderId");
            AddPrimaryKey("dbo.Clients", "ClientId");
            RenameIndex(table: "dbo.Telephones", name: "IX_PersonId", newName: "IX_ClientId");
            RenameColumn(table: "dbo.Telephones", name: "PersonId", newName: "ClientId");
            AddForeignKey("dbo.Shopping", "ProviderId", "dbo.Providers", "ProviderId");
            AddForeignKey("dbo.clientSales", "clientId", "dbo.Clients", "ClientId", cascadeDelete: true);
            AddForeignKey("dbo.Telephones", "ClientId", "dbo.Clients", "ClientId", cascadeDelete: true);
        }
    }
}
