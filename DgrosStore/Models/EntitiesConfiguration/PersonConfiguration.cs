using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace DgrosStore.Models.EntitiesConfiguration 
{
    public class PersonConfiguration : EntityTypeConfiguration<Person>
    {
        public PersonConfiguration()
        {
            //personas
            ToTable("People");
            HasKey(p => p.PersonId);

            Property(p => p.Name)
                .IsRequired();

            Property(p => p.Type)
                .IsRequired();

            Property(p => p.Email)
                .IsRequired();

        }
    }
}