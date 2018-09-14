namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctionToPersonTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.People", "Direcction", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Direcction", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
