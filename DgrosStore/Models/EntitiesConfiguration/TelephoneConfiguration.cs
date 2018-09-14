using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class TelephoneConfiguration :EntityTypeConfiguration<Telephone>
    {
        public TelephoneConfiguration()
        {
            ToTable("Telephone");
            HasKey(t => t.TelephoneId);


            HasRequired(c => c.Person)
            .WithMany(t => t.Telephones)
            .HasForeignKey(c => c.PersonId);
            
        }
    }
}