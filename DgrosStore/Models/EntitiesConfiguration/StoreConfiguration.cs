using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class StoreConfiguration :EntityTypeConfiguration<Store>
    {
        public StoreConfiguration()
        {
            //tienda

            ToTable("Store");
            HasKey(s => s.StoreId);

            Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired();

            Property(s => s.Telephone)
                .IsRequired();
        }
    }
}