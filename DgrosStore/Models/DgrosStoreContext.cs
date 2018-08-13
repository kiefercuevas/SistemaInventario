using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DgrosStore.Models.EntitiesConfiguration;
namespace DgrosStore.Models
{
    public class DgrosStoreContext :DbContext
    {
        public DgrosStoreContext()
            :base("name=DgrosStoreConection")
        {   }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Shopping> Shoppings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Telephone> Telephones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new ProviderConfiguration());
            modelBuilder.Configurations.Add(new SalesConfigutation());
            modelBuilder.Configurations.Add(new SalesProductConfiguration());
            modelBuilder.Configurations.Add(new ShoppingConfiguration());
            modelBuilder.Configurations.Add(new ShoppingProductConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());


        }

    }
}