namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageColumnToClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "Image");
        }
    }
}
