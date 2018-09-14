namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientModificationClientIdColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "ClientId", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "ClientId", c => c.Int(nullable: false));
        }
    }
}
