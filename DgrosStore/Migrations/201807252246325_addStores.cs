namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStores : DbMigration
    {
        public override void Up()
        {
            var tableName = "Store";
            var url = @"~/Content/Images/prueba.jpg";
            Sql(string.Format("INSERT INTO {0} Values " +
                "('DgroStore'," +
                "'Calle primera #8 urb. italia'," +
                "'809-903-9035'," +
                "'{1}'," +
                "'Ropa exclusiva para damas y caballeros')",tableName,url)
                );
        }
        
        public override void Down()
        {
            Sql("TRUNCATE TABLE Store");
        }
    }
}
