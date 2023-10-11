using Backand.DbEntites;
using Microsoft.EntityFrameworkCore;

namespace Backand
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Mine> Mine { get; set; }
        public DbSet<Objects> Objects { get; set; }
        public DbSet <Construction> Construction { get; set; }

       public DbSet <Objects_Construction> Objects_Construction { get; set; }

        public DbSet <ConstructionUnitType> ConstructionUnitType { get; set; }

        public DbSet<ConstructionUnit> ConstructionUnit { get; set; }

        public DbSet<MaterialSet> MaterialSet { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; } 
 
        public DbSet<LogisticCompany> LogisticCompany { get; set; }
        public DbSet<Storage> Storage { get; set; }
        public DbSet<CompanyType> CompanyType { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<TransportType> TransportType { get; set; }

        public DbSet<TransportMode> TransportMode { get; set; }

        public DbSet<CoefficientType> CoefficientType { get; set; }

        public DbSet<CompanyTransport> CompanyTransport { get; set; }

        public DbSet<MaterialSet_ConstructionUnit> MaterialSet_ConstructionUnit { get; set; }

        public DbSet<Storage_ConstructionUnit> Storage_ConstructionUnit { get; set; }

        public DbSet<DeliveryAbility> DeliveryAbility { get; set; }
        public DbSet<UserType> UserType { get; set; }

        public DbSet<User> User { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gazprom_db;Username=postgres;Password=admin");
        }
    }
}