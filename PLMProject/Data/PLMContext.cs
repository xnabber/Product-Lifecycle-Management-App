using Microsoft.EntityFrameworkCore;
using PLMApp.Models;

namespace PLMApp.Data
{
    public class PLMContext : DbContext
    {
        public PLMContext(DbContextOptions<PLMContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Bom> Boms { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<BomMaterial> BomMaterials { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<ProductStageHistory> ProductStageHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BomMaterial>().HasKey(bm => new { bm.BomId, bm.MaterialNumber });
            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.RoleId, ur.UserId });
            modelBuilder.Entity<ProductStageHistory>().Property(p => p.StartOfStage).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<BomMaterial>().HasKey(bm => new { bm.BomId, bm.MaterialNumber });
            modelBuilder.Entity<ProductStageHistory>().HasKey(psh => new { psh.ProductId, psh.StageId, psh.StartOfStage });

            base.OnModelCreating(modelBuilder);
        }
    }
}
