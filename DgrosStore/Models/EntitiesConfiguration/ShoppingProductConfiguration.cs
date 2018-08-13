using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ShoppingProductConfiguration :EntityTypeConfiguration<ShoppingProducts>
    {
        public ShoppingProductConfiguration()
        {

            // tabla intermedia comprasProductos
            ToTable("ShoppingProducts");
            HasKey(s => new { s.ProductId, s.ShoppingId });
        }
    }
}