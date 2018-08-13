using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ShoppingConfiguration :EntityTypeConfiguration<Shopping>
    {
        public ShoppingConfiguration()
        {
            //compras

            ToTable("Shopping");
            HasKey(s => s.Id);

                HasRequired(c => c.Provider)
                .WithMany(s => s.Shoppings)
                .HasForeignKey(c => c.ProviderId)
                .WillCascadeOnDelete(false);

                HasRequired(c => c.Store)
                .WithMany(s => s.Shoppings)
                .HasForeignKey(c => c.StoreId)
                .WillCascadeOnDelete(false);
               
        }
    }
}