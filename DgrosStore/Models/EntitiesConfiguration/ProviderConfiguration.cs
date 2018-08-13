using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ProviderConfiguration :EntityTypeConfiguration<Provider>
    {
        public ProviderConfiguration()
        {
            //proveedores
            ToTable("Providers");
            HasKey(p => p.ProviderId);


            Property(p => p.Name)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}