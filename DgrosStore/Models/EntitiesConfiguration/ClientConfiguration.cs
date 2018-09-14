using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
        {
            //clientes

            ToTable("Clients");
            HasKey(c => c.ClientId);


            Property(c => c.LastName)
                .HasMaxLength(30)
                .IsRequired();

            Property(c => c.ClientId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        }
    }
}