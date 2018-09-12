using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
namespace DgrosStore.Models.EntitiesConfiguration
{
    public class CategoryConfiguration :EntityTypeConfiguration<Category>
    {
        public CategoryConfiguration()
        {
            ToTable("Categories");
            HasKey(c => c.CategoryId);
            
                    Property(c => c.Name)
                    .IsRequired();

            Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}