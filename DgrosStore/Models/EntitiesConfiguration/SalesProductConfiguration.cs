using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
namespace DgrosStore.Models.EntitiesConfiguration
{
    public class SalesProductConfiguration : EntityTypeConfiguration<SalesProducs>
    {
        public SalesProductConfiguration()
        {
            ////tabla intermedia VentaProductos
            ToTable("SalesProducts");
            HasKey(s => new { s.ProductId, s.SalesId });
        }
    }
}