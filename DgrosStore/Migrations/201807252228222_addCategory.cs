namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCategory : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO categories Values ('Ropa')");
            Sql("INSERT INTO categories Values ('Collares')");
            Sql("INSERT INTO categories Values ('Zapatos')");
            Sql("INSERT INTO categories Values ('Cremas')");
            Sql("INSERT INTO categories Values ('Maquillaje')");
        }
        
        public override void Down()
        {
            Sql("TRUNCATE TABLE categories");
        }
    }
}
