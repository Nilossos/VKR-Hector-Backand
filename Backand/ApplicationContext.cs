using Backand.DbEntities;
using Backand.DbEntities.ConstructionSpace;
using Microsoft.EntityFrameworkCore;

namespace Backand
{

    public partial class ApplicationContext : DbContext
	{
		public ApplicationContext()
		{
		}

		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}

		public virtual DbSet<CoefficientType> CoefficientType { get; set; }

		public virtual DbSet<Company> Company { get; set; }

		public virtual DbSet<CompanyType> CompanyType { get; set; }

		public virtual DbSet<Construction> Construction { get; set; }

		public virtual DbSet<ConstructionState> ConstructionState { get; set; }

		public virtual DbSet<ConstructionType> ConstructionType { get; set; }

		public virtual DbSet<ConstructionUnit> ConstructionUnit { get; set; }

		public virtual DbSet<ConstructionUnitType> ConstructionUnitType { get; set; }

		public virtual DbSet<DeliveryRegion> DeliveryRegion { get; set; }

		public virtual DbSet<LogisticCompany> LogisticCompany { get; set; }

		public virtual DbSet<Manufacturer> Manufacturer { get; set; }

		public virtual DbSet<MaterialSet> MaterialSet { get; set; }

		public virtual DbSet<MaterialSet_ConstructionUnit> MaterialSet_ConstructionUnit { get; set; }

		public virtual DbSet<MeasureUnit> MeasureUnit { get; set; }

		public virtual DbSet<Mine> Mine { get; set; }

		public virtual DbSet<Objects> Objects { get; set; }

		public virtual DbSet<ObjectsTransportType> ObjectsTransportType { get; set; }

		public virtual DbSet<Region> Region { get; set; }

		public virtual DbSet<Storage> Storage { get; set; }

		public virtual DbSet<Storage_ConstructionUnit> Storage_ConstructionUnit { get; set; }

        public virtual DbSet<StorageToObjectsDistance> StorageToObjectDistance { get; set; }

        public virtual DbSet<StorageToTransportFleetDistance> StorageToTransportFleetDistance { get; set; }

        public virtual DbSet<Subsidiary> Subsidiary { get; set; }

		public virtual DbSet<Transport> Transport { get; set; }

		public virtual DbSet<TransportFleet> TransportFleet { get; set; }

		public virtual DbSet<TransportFleet_Transport> TransportFleet_Transport { get; set; }

        public virtual DbSet<TransportFleetToObjectsDistance> TransportFleetToObjectsDistance { get; set; }

        public virtual DbSet<TransportMode> TransportMode { get; set; }

		public virtual DbSet<TransportType> TransportType { get; set; }

		public virtual DbSet<User> User { get; set; }

		public virtual DbSet<UserType> UserType { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
			=> optionsBuilder.UseNpgsql("Host=localhost;Database=gazprom_db;Username=postgres;Password=postgres");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<CoefficientType>(entity =>
			{
				entity.HasKey(e => e.CoefficientTypeId).HasName("CoefficientType_pkey");

				entity.ToTable("CoefficientType");

				entity.Property(e => e.CoefficientTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(30);
			});

			modelBuilder.Entity<Company>(entity =>
			{
				entity.HasKey(e => e.CompanyId).HasName("Company_pkey");

				entity.ToTable("Company");

				entity.Property(e => e.CompanyId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Email).HasMaxLength(50);
				entity.Property(e => e.PhoneNumber).HasMaxLength(20);
				entity.Property(e => e.Url).HasMaxLength(200);

				entity.HasOne(d => d.CompanyType).WithMany(p => p.Companies)
					.HasForeignKey(d => d.CompanyTypeId)
					.HasConstraintName("Company_CompanyTypeId_fkey");
			});

			modelBuilder.Entity<CompanyType>(entity =>
			{
				entity.HasKey(e => e.CompanyTypeId).HasName("CompanyType_pkey");

				entity.ToTable("CompanyType");

				entity.Property(e => e.CompanyTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);
			});

			modelBuilder.Entity<Construction>(entity =>
			{
				entity.HasKey(e => e.ConstructionId).HasName("Construction_pkey");

				entity.ToTable("Construction");

				entity.Property(e => e.ConstructionId).UseIdentityAlwaysColumn();
				entity.Property(e => e.ConstructionName).HasMaxLength(100);

				entity.HasOne(d => d.ConstructionState).WithMany(p => p.Constructions)
					.HasForeignKey(d => d.ConstructionStateId)
					.HasConstraintName("Construction_ConstructionStateId_fkey");

				entity.HasOne(d => d.ConstructionType).WithMany(p => p.Constructions)
					.HasForeignKey(d => d.ConstructionTypeId)
					.HasConstraintName("Construction_ConstructionTypeId_fkey");

				entity.HasOne(d => d.Object).WithMany(p => p.Constructions)
					.HasForeignKey(d => d.ObjectsId)
					.HasConstraintName("Construction_ObjectsId_fkey");
			});

			modelBuilder.Entity<ConstructionState>(entity =>
			{
				entity.HasKey(e => e.ConstructionStateId).HasName("ConstructionState_pkey");

				entity.ToTable("ConstructionState");

				entity.Property(e => e.ConstructionStateId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<ConstructionType>(entity =>
			{
				entity.HasKey(e => e.ConstructionTypeId).HasName("ConstructionType_pkey");

				entity.ToTable("ConstructionType");

				entity.Property(e => e.ConstructionTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.DocumentPath).HasColumnType("character varying");
				entity.Property(e => e.Name).HasMaxLength(100);
			});

			modelBuilder.Entity<ConstructionUnit>(entity =>
			{
				entity.HasKey(e => e.ConstructionUnitId).HasName("ConstructionUnit_pkey");

				entity.ToTable("ConstructionUnit");

				entity.Property(e => e.ConstructionUnitId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.ConstructionUnitType).WithMany(p => p.ConstructionUnits)
					.HasForeignKey(d => d.ConstructionUnitTypeId)
					.HasConstraintName("ConstructionUnit_ConstructionUnitTypeId_fkey");

				entity.HasOne(d => d.MeasureUnit).WithMany(p => p.ConstructionUnits)
					.HasForeignKey(d => d.MeasureUnitId)
					.HasConstraintName("ConstructionUnit_MeasureUnitId_fkey");
			});

			modelBuilder.Entity<ConstructionUnitType>(entity =>
			{
				entity.HasKey(e => e.ConstructionUnitTypeId).HasName("ConstructionUnitType_pkey");

				entity.ToTable("ConstructionUnitType");

				entity.Property(e => e.ConstructionUnitTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(50);
			});

			modelBuilder.Entity<DeliveryRegion>(entity =>
			{
				entity.HasKey(e => e.DeliveryRegionId).HasName("DeliveryRegion_pkey");

				entity.ToTable("DeliveryRegion");

				entity.Property(e => e.DeliveryRegionId).UseIdentityAlwaysColumn();
				entity.Property(e => e.TransportFleet_TransportId).HasColumnName("TransportFleet_TransportId");

				entity.HasOne(d => d.Region).WithMany(p => p.DeliveryRegions)
					.HasForeignKey(d => d.RegionId)
					.HasConstraintName("DeliveryRegion_RegionId_fkey");

				entity.HasOne(d => d.TransportFleet_Transport).WithMany(p => p.DeliveryRegions)
					.HasForeignKey(d => d.TransportFleet_TransportId)
					.HasConstraintName("DeliveryRegion_TransportFleet_TransportId_fkey");
			});

			modelBuilder.Entity<LogisticCompany>(entity =>
			{
				entity.HasKey(e => e.LogisticCompanyId).HasName("LogisticCompany_pkey");

				entity.ToTable("LogisticCompany");

				entity.Property(e => e.LogisticCompanyId).ValueGeneratedNever();
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.LogisticCompanyNavigation).WithOne(p => p.LogisticCompany)
					.HasForeignKey<LogisticCompany>(d => d.LogisticCompanyId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("LogisticCompany_LogisticCompanyId_fkey");
			});

			modelBuilder.Entity<Manufacturer>(entity =>
			{
				entity.HasKey(e => e.ManufacturerId).HasName("Manufacturer_pkey");

				entity.ToTable("Manufacturer");

				entity.Property(e => e.ManufacturerId).ValueGeneratedNever();
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.ManufacturerNavigation).WithOne(p => p.Manufacturer)
					.HasForeignKey<Manufacturer>(d => d.ManufacturerId)
					.OnDelete(DeleteBehavior.ClientSetNull)
					.HasConstraintName("Manufacturer_ManufacturerId_fkey");
			});

			modelBuilder.Entity<MaterialSet>(entity =>
			{
				entity.HasKey(e => e.MaterialSetId).HasName("MaterialSet_pkey");

				entity.ToTable("MaterialSet");

				entity.Property(e => e.MaterialSetId).UseIdentityAlwaysColumn();

				entity.HasOne(d => d.ConstructionType).WithMany(p => p.MaterialSets)
					.HasForeignKey(d => d.ConstructionTypeId)
					.HasConstraintName("MaterialSet_ConstructionTypeId_fkey");
			});

			modelBuilder.Entity<MaterialSet_ConstructionUnit>(entity =>
			{
				entity.HasKey(e => e.MaterialSet_ConstructionUnitId).HasName("MaterialSet_ConstructionUnit_pkey");

				entity.ToTable("MaterialSet_ConstructionUnit");

				entity.Property(e => e.MaterialSet_ConstructionUnitId)
					.UseIdentityAlwaysColumn()
					.HasColumnName("MaterialSet_ConstructionUnitId");

				entity.HasOne(d => d.ConstructionUnit).WithMany(p => p.MaterialSet_ConstructionUnits)
					.HasForeignKey(d => d.ConstructionUnitId)
					.HasConstraintName("MaterialSet_ConstructionUnit_ConstructionUnitId_fkey");

				entity.HasOne(d => d.MaterialSet).WithMany(p => p.MaterialSet_ConstructionUnits)
					.HasForeignKey(d => d.MaterialSetId)
					.HasConstraintName("MaterialSet_ConstructionUnit_MaterialSetId_fkey");
			});

			modelBuilder.Entity<MeasureUnit>(entity =>
			{
				entity.HasKey(e => e.MeasureUnitId).HasName("MeasureUnit_pkey");

				entity.ToTable("MeasureUnit");

				entity.Property(e => e.MeasureUnitId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(80);
			});

			modelBuilder.Entity<Mine>(entity =>
			{
				entity.HasKey(e => e.MineId).HasName("Mine_pkey");

				entity.ToTable("Mine");

				entity.Property(e => e.MineId).UseIdentityAlwaysColumn();
				entity.Property(e => e.DocumentPath).HasColumnType("character varying");
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.Subsidiary).WithMany(p => p.Mines)
					.HasForeignKey(d => d.SubsidiaryId)
					.HasConstraintName("Mine_SubsidiaryId_fkey");
			});

			modelBuilder.Entity<Objects>(entity =>
			{
				entity.HasKey(e => e.ObjectsId).HasName("Objects_pkey");

				entity.Property(e => e.ObjectsId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.Mine).WithMany(p => p.Objects)
					.HasForeignKey(d => d.MineId)
					.HasConstraintName("Objects_MineId_fkey");

				entity.HasOne(d => d.Region).WithMany(p => p.Objects)
					.HasForeignKey(d => d.RegionId)
					.HasConstraintName("Objects_RegionId_fkey");
			});

			modelBuilder.Entity<ObjectsTransportType>(entity =>
			{
				entity.HasKey(e => e.ObjectsTransportTypeId).HasName("Objects_TransportType_pkey");

				entity.ToTable("Objects_TransportType");

				entity.Property(e => e.ObjectsTransportTypeId)
					.UseIdentityAlwaysColumn()
					.HasColumnName("Objects_TransportTypeId");

				entity.HasOne(d => d.Objects).WithMany(p => p.ObjectsTransportTypes)
					.HasForeignKey(d => d.ObjectsId)
					.HasConstraintName("Objects_TransportType_ObjectsId_fkey");

				entity.HasOne(d => d.TransportType).WithMany(p => p.ObjectsTransportTypes)
					.HasForeignKey(d => d.TransportTypeId)
					.HasConstraintName("Objects_TransportType_TransportTypeId_fkey");
			});

			modelBuilder.Entity<Region>(entity =>
			{
				entity.HasKey(e => e.RegionId).HasName("Region_pkey");

				entity.ToTable("Region");

				entity.Property(e => e.RegionId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(80);
			});

			modelBuilder.Entity<Storage>(entity =>
			{
				entity.HasKey(e => e.StorageId).HasName("Storage_pkey");

				entity.ToTable("Storage");

				entity.Property(e => e.StorageId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Address).HasMaxLength(200);
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.Manufacturer).WithMany(p => p.Storages)
					.HasForeignKey(d => d.ManufacturerId)
					.HasConstraintName("Storage_ManufacturerId_fkey");

				entity.HasOne(d => d.Region).WithMany(p => p.Storages)
					.HasForeignKey(d => d.RegionId)
					.HasConstraintName("Storage_RegionId_fkey");
			});

			modelBuilder.Entity<Storage_ConstructionUnit>(entity =>
			{
				entity.HasKey(e => e.Storage_ConstructionUnitId).HasName("Storage_ConstructionUnit_pkey");

				entity.ToTable("Storage_ConstructionUnit");

				entity.Property(e => e.Storage_ConstructionUnitId)
					.UseIdentityAlwaysColumn()
					.HasColumnName("Storage_ConstructionUnitId");
				entity.Property(e => e.DocumentPath).HasColumnType("character varying");
				entity.Property(e => e.PricePerUnit).HasPrecision(13, 2);
				entity.Property(e => e.TablePath).HasColumnType("character varying");

				entity.HasOne(d => d.ConstructionUnit).WithMany(p => p.Storage_ConstructionUnits)
					.HasForeignKey(d => d.ConstructionUnitId)
					.HasConstraintName("Storage_ConstructionUnit_ConstructionUnitId_fkey");

				entity.HasOne(d => d.Storage).WithMany(p => p.Storage_ConstructionUnits)
					.HasForeignKey(d => d.StorageId)
					.HasConstraintName("Storage_ConstructionUnit_StorageId_fkey");
			});

			modelBuilder.Entity<Subsidiary>(entity =>
			{
				entity.HasKey(e => e.SubsidiaryId).HasName("Subsidiary_pkey");

				entity.ToTable("Subsidiary");

				entity.Property(e => e.SubsidiaryId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);
			});

			modelBuilder.Entity<Transport>(entity =>
			{
				entity.HasKey(e => e.TransportId).HasName("Transport_pkey");

				entity.ToTable("Transport");

				entity.Property(e => e.TransportId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(80);

				entity.HasOne(d => d.TransportMode).WithMany(p => p.Transports)
					.HasForeignKey(d => d.TransportModeId)
					.HasConstraintName("Transport_TransportModeId_fkey");
			});

			modelBuilder.Entity<TransportFleet>(entity =>
			{
				entity.HasKey(e => e.TransportFleetId).HasName("TransportFleet_pkey");

				entity.ToTable("TransportFleet");

				entity.Property(e => e.TransportFleetId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Address).HasMaxLength(200);
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.Company).WithMany(p => p.TransportFleets)
					.HasForeignKey(d => d.CompanyId)
					.HasConstraintName("TransportFleet_CompanyId_fkey");

				entity.HasOne(d => d.Region).WithMany(p => p.TransportFleets)
					.HasForeignKey(d => d.RegionId)
					.HasConstraintName("TransportFleet_RegionId_fkey");
			});

			modelBuilder.Entity<TransportFleet_Transport>(entity =>
			{
				entity.HasKey(e => e.TransportFleet_TransportId).HasName("TransportFleet_Transport_pkey");

				entity.ToTable("TransportFleet_Transport");

				entity.Property(e => e.TransportFleet_TransportId)
					.UseIdentityAlwaysColumn()
					.HasColumnName("TransportFleet_TransportId");

				entity.HasOne(d => d.CoefficientType).WithMany(p => p.TransportFleet_Transports)
					.HasForeignKey(d => d.CoefficientTypeId)
					.HasConstraintName("TransportFleet_Transport_CoefficientTypeId_fkey");

				entity.HasOne(d => d.TransportFleet).WithMany(p => p.TransportFleet_Transports)
					.HasForeignKey(d => d.TransportFleetId)
					.HasConstraintName("TransportFleet_Transport_TransportFleetId_fkey");

				entity.HasOne(d => d.Transport).WithMany(p => p.TransportFleet_Transports)
					.HasForeignKey(d => d.TransportId)
					.HasConstraintName("TransportFleet_Transport_TransportId_fkey");
			});

			modelBuilder.Entity<TransportMode>(entity =>
			{
				entity.HasKey(e => e.TransportModeId).HasName("TransportMode_pkey");

				entity.ToTable("TransportMode");

				entity.Property(e => e.TransportModeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);

				entity.HasOne(d => d.TransportType).WithMany(p => p.TransportModes)
					.HasForeignKey(d => d.TransportTypeId)
					.HasConstraintName("TransportMode_TransportTypeId_fkey");
			});

			modelBuilder.Entity<TransportType>(entity =>
			{
				entity.HasKey(e => e.TransportTypeId).HasName("TransportType_pkey");

				entity.ToTable("TransportType");

				entity.Property(e => e.TransportTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(100);
			});

			modelBuilder.Entity<User>(entity =>
			{
				entity
					.HasNoKey()
					.ToTable("User");

				entity.Property(e => e.FirstName).HasMaxLength(30);
				entity.Property(e => e.Login).HasMaxLength(100);
				entity.Property(e => e.Password).HasMaxLength(64);
				entity.Property(e => e.Patronymic).HasMaxLength(30);
				entity.Property(e => e.PhoneNumber).HasMaxLength(20);
				entity.Property(e => e.PhotoPath).HasColumnType("character varying");
				entity.Property(e => e.Surname).HasMaxLength(30);
				entity.Property(e => e.Token).HasMaxLength(300);
				entity.Property(e => e.UserId)
					.ValueGeneratedOnAdd()
					.UseIdentityAlwaysColumn();

				entity.HasOne(d => d.Subsidiary).WithMany()
					.HasForeignKey(d => d.SubsidiaryId)
					.HasConstraintName("User_SubsidiaryId_fkey");

				entity.HasOne(d => d.UserType).WithMany()
					.HasForeignKey(d => d.UserTypeId)
					.HasConstraintName("User_UserTypeId_fkey");
			});

			modelBuilder.Entity<UserType>(entity =>
			{
				entity.HasKey(e => e.UserTypeId).HasName("UserType_pkey");

				entity.ToTable("UserType");

				entity.Property(e => e.UserTypeId).UseIdentityAlwaysColumn();
				entity.Property(e => e.Name).HasMaxLength(30);
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}