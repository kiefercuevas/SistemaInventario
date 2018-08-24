namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createIdCardColunmIntoClientTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "IdCard", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "IdCard");
        }
    }
}
