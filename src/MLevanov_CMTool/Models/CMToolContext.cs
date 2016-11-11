using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;

namespace MLevanov_CMTool.Models
{
    public class CmToolContext: DbContext
    {
        public CmToolContext()
        {

            //Database.EnsureDeleted();
        }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Shelve> Shelves { get; set; }
        public DbSet<Good> Goods { get; set; }
        public DbSet<GoodsClass> GoodsClasses { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Data:CmToolConnection"];
            optionsBuilder.UseSqlServer(connString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder cmModelBuilder)
        {
            cmModelBuilder.Entity<Shelve>()
                .HasOne(p => p.ShelveGoodsClass)
                .WithMany(p => p.GoodClassShelve);
            cmModelBuilder.Entity<Shelve>()
                .HasOne(p => p.ShelveStore)
                .WithMany(p => p.StoreShelves);
            cmModelBuilder.Entity<Sale>()
               .HasOne(p => p.SalesGood)
               .WithMany(p => p.GoodSales);
            cmModelBuilder.Entity<Sale>()
                .HasOne(p => p.SalesStore)
                .WithMany(p => p.StoreSales);
        }
    }
}
