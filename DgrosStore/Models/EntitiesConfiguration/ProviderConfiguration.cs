using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ProviderConfiguration :EntityTypeConfiguration<Provider>
    {
        public ProviderConfiguration()
        {
            //proveedores
            ToTable("Providers");
            HasKey(p => p.ProviderId);

            Property(p => p.ProviderId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}