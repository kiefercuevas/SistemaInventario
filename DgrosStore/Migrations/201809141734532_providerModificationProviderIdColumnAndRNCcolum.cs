namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class providerModificationProviderIdColumnAndRNCcolum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Providers", "ProviderId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Providers", "Rnc", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Providers", "Rnc", c => c.String());
            AlterColumn("dbo.Providers", "ProviderId", c => c.Int(nullable: false));
        }
    }
}
