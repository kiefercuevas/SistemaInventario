using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;

namespace DgrosStore.Models.EntitiesConfiguration
{
    public class ProductConfiguration :EntityTypeConfiguration<Product>
    {
        public ProductConfiguration()
        {
            //productos

            ToTable("Products");
            HasKey(p => p.ProductId);


            Property(p => p.Name)
                .IsRequired();

            Property(p => p.Description)
                .IsMaxLength();

            HasRequired(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId);
        }
    }
}