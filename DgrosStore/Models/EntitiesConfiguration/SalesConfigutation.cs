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

            HasMany(c => c.Clients)
                .WithMany(s => s.Sales)
                .Map(m =>
                {
                    m.ToTable("clientSales");
                    m.MapLeftKey("salesId");
                    m.MapRightKey("clientId");
                });

            HasRequired(st => st.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(st => st.StoreId)
                .WillCascadeOnDelete(false);
        }
    }
}