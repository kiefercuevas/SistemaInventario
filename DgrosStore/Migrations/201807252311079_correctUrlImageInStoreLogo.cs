namespace DgrosStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctUrlImageInStoreLogo : DbMigration
    {
        public override void Up()
        {
            var tableName = "Store";
            var url = @"/Content/Images/prueba.jpg";
            Sql(string.Format("UPDATE {0} SET Name  = 'DgroStore'," +
                "Address = 'Calle primera #8 urb. italia'," +
                "Telephone = '809-903-9035'," +
                "Logo = '{1}'," +
                "Slogan = 'Ropa exclusiva para damas y caballeros' WHERE StoreId = 1", tableName,url));
         
        }
        
        public override void Down()
        {
            var tableName = "Store";
            var url = @"~/Content/Images/prueba.jpg";
            Sql(string.Format("UPDATE {0} SET Name  = 'DgroStore'," +
                "Address = 'Calle primera #8 urb. italia'," +
                "Telephone = '809-903-9035'," +
                "Logo = '{1}'," +
                "Slogan = 'Ropa exclusiva para damas y caballeros' WHERE StoreId = 1", tableName, url));
        }
    }
}
