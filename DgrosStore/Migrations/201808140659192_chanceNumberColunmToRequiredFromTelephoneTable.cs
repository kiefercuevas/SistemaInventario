namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chanceNumberColunmToRequiredFromTelephoneTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Telephones", "Number", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Telephones", "Number", c => c.String());
        }
    }
}
