using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class SalesConfigutation :EntityTypeConfiguration<Sales>
    {
        public SalesConfigutation()
        {
            //ventas

            ToTable("Sales");
            HasKey(s => s.Id);

            HasRequired(c => c.Client)
                .WithMany(s => s.Sales)
                .HasForeignKey(c => c.ClientId)
                .WillCascadeOnDelete(false);

            HasRequired(st => st.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(st => st.StoreId)
                .WillCascadeOnDelete(false);
        }
    }
}