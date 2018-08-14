namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTelephoneColumnToRequiredFromProviderTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Providers", "Telephone", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Providers", "Telephone", c => c.String());
        }
    }
}
