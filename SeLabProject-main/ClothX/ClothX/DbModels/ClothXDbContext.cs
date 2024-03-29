using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ClothX.DbModels
{
    public partial class ClothXDbContext : DbContext
    {
        public ClothXDbContext()
        {
        }

        public ClothXDbContext(DbContextOptions<ClothXDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<AspNetUsersAudit> AspNetUsersAudits { get; set; } = null!;
        public virtual DbSet<ClientOrder> ClientOrders { get; set; } = null!;
        public virtual DbSet<ClientOrderIdeaImage> ClientOrderIdeaImages { get; set; } = null!;
        public virtual DbSet<ClientOrderIdeaImagesAudit> ClientOrderIdeaImagesAudits { get; set; } = null!;
        public virtual DbSet<ClientOrdersAudit> ClientOrdersAudits { get; set; } = null!;
        public virtual DbSet<Feature> Features { get; set; } = null!;
        public virtual DbSet<FeatureGroup> FeatureGroups { get; set; } = null!;
        public virtual DbSet<FeatureGroupsAudit> FeatureGroupsAudits { get; set; } = null!;
        public virtual DbSet<FeaturesAudit> FeaturesAudits { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Lookup> Lookups { get; set; } = null!;
        public virtual DbSet<LookupAudit> LookupAudits { get; set; } = null!;
        public virtual DbSet<OrderFeature> OrderFeatures { get; set; } = null!;
        public virtual DbSet<OrderFeaturesAudit> OrderFeaturesAudits { get; set; } = null!;
        public virtual DbSet<Prevelige> Preveliges { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<ProductCategoryAudit> ProductCategoryAudits { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<ReviewAudit> ReviewAudits { get; set; } = null!;
        public virtual DbSet<TailorProject> TailorProjects { get; set; } = null!;
        public virtual DbSet<TailorProjectImage> TailorProjectImages { get; set; } = null!;
        public virtual DbSet<TailorProjectImagesAudit> TailorProjectImagesAudits { get; set; } = null!;
        public virtual DbSet<TailorProjectsAudit> TailorProjectsAudits { get; set; } = null!;
        public virtual DbSet<UserProfile> UserProfiles { get; set; } = null!;
        public virtual DbSet<UserProfileAudit> UserProfileAudits { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer("Server=(local); Database=ClothXDb; Integrated Security=True; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);

                entity.HasMany(d => d.Preveliges)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "RolePrevelige",
                        l => l.HasOne<Prevelige>().WithMany().HasForeignKey("PreveligeId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolePrevelige_Prevelige"),
                        r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_RolePrevelige_AspNetRoles"),
                        j =>
                        {
                            j.HasKey("RoleId", "PreveligeId");

                            j.ToTable("RolePrevelige");

                            j.IndexerProperty<string>("RoleId").HasMaxLength(128).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AspNetUserRoles_AspNetRoles"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_AspNetUserRoles_AspNetUsers"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.IndexerProperty<string>("UserId").HasMaxLength(128);

                            j.IndexerProperty<string>("RoleId").HasMaxLength(128).IsUnicode(false);
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsersAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__AspNetUs__A17F239804FE5EF9");

                entity.ToTable("AspNetUsers_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Id).HasMaxLength(128);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<ClientOrder>(entity =>
            {
                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsConfirmed).HasColumnName("isConfirmed");

                entity.Property(e => e.IsDelivered).HasColumnName("isDelivered");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.Measurements).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientOrders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientOrders_UserProfile");

                entity.HasOne(d => d.OrderTypeNavigation)
                    .WithMany(p => p.ClientOrders)
                    .HasForeignKey(d => d.OrderType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientOrders_ProductCategory");
            });

            modelBuilder.Entity<ClientOrderIdeaImage>(entity =>
            {
                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ClientOrderIdeaImages)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClientOrderIdeaImages_ClientOrders");
            });

            modelBuilder.Entity<ClientOrderIdeaImagesAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__ClientOr__A17F23980981DA5B");

                entity.ToTable("ClientOrderIdeaImages_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");
            });

            modelBuilder.Entity<ClientOrdersAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__ClientOr__A17F2398C813E709");

                entity.ToTable("ClientOrders_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsConfirmed).HasColumnName("isConfirmed");

                entity.Property(e => e.IsDelivered).HasColumnName("isDelivered");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.Measurements).HasMaxLength(200);

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Feature>(entity =>
            {
                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.FeatureGroup)
                    .WithMany(p => p.Features)
                    .HasForeignKey(d => d.FeatureGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Features_FeatureGroups");
            });

            modelBuilder.Entity<FeatureGroup>(entity =>
            {
                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FeatureGroupsAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__FeatureG__A17F2398C7853B06");

                entity.ToTable("FeatureGroups_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<FeaturesAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Features__A17F239817C97F54");

                entity.ToTable("Features_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Log");

                entity.Property(e => e.Level).HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Logger).HasMaxLength(250);

                entity.Property(e => e.MachineName).HasMaxLength(50);
            });

            modelBuilder.Entity<Lookup>(entity =>
            {
                entity.ToTable("Lookup");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

                entity.Property(e => e.Value).HasMaxLength(100);
            });

            modelBuilder.Entity<LookupAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Lookup_A__A17F2398D057A214");

                entity.ToTable("Lookup_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.DisplayOrder).HasColumnName("displayOrder");

                entity.Property(e => e.Value).HasMaxLength(100);
            });

            modelBuilder.Entity<OrderFeature>(entity =>
            {
                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Feature)
                    .WithMany(p => p.OrderFeatures)
                    .HasForeignKey(d => d.FeatureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderFeatures_Features");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderFeatures)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderFeatures_ClientOrders");
            });

            modelBuilder.Entity<OrderFeaturesAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__OrderFea__A17F239866D6E1EF");

                entity.ToTable("OrderFeatures_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.IsActive).HasColumnName("isActive");
            });

            modelBuilder.Entity<Prevelige>(entity =>
            {
                entity.ToTable("Prevelige");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<ProductCategoryAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__ProductC__A17F23985018D198");

                entity.ToTable("ProductCategory_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.ResponseAddedOn).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_UserProfile");
            });

            modelBuilder.Entity<ReviewAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__Review_A__A17F2398E859A2C4");

                entity.ToTable("Review_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(100);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.ResponseAddedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<TailorProject>(entity =>
            {
                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TailorProjects)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TailorProjects_ProductCategory");
            });

            modelBuilder.Entity<TailorProjectImage>(entity =>
            {
                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TailorProjectImages)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TailorProjectImages_TailorProjects");
            });

            modelBuilder.Entity<TailorProjectImagesAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__TailorPr__A17F23980FEA0A92");

                entity.ToTable("TailorProjectImages_Audit", "Audit");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");
            });

            modelBuilder.Entity<TailorProjectsAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__TailorPr__A17F23981ACAF33C");

                entity.ToTable("TailorProjects_Audit", "Audit");

                entity.Property(e => e.AddedBy).HasMaxLength(50);

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.Title).HasMaxLength(255);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.ToTable("UserProfile");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProfiles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_AspNetUsers");
            });

            modelBuilder.Entity<UserProfileAudit>(entity =>
            {
                entity.HasKey(e => e.AuditId)
                    .HasName("PK__UserProf__A17F2398F1F14365");

                entity.ToTable("UserProfile_Audit", "Audit");

                entity.Property(e => e.AddedOn).HasColumnType("datetime");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.AuditDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.AuditOperation).HasMaxLength(10);

                entity.Property(e => e.AuditUser)
                    .HasMaxLength(128)
                    .HasDefaultValueSql("([dbo].[GetCurrentUser]())");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(128);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
