using DigitalDistribution.Models.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DigitalDistribution.Models.Database
{
    public class DigitalDistributionDbContext : IdentityDbContext<UserEntity, RoleEntity, int, IdentityUserClaim<int>, UserRoleEntity,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>

    {
        public DigitalDistributionDbContext(DbContextOptions<DigitalDistributionDbContext> options) : base(options)
        {
        }

        public DbSet<BillingAddressEntity> Addresses { get; set; }
        public DbSet<DevelopmentTeamEntity> Developers { get; set; }
        public DbSet<InvoiceEntity> Invoices { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProfileEntity> Profiles { get; set; }
        public DbSet<UpdateEntity> Updates { get; set; }


        public DbSet<CheckoutItemEntity> InvoiceItems { get; set; }
        public DbSet<LibraryProductEntity> LibraryItems { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region 1:1 Relationships

            modelBuilder.Entity<UserEntity>()
                .HasOne(e => e.Address)
                .WithOne(e => e.User)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasOne(e => e.Profile)
                .WithOne(e => e.User)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region 1:N Relationships

            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.Bills)
                .WithOne(e => e.User)
                .HasForeignKey(fk => fk.UserId)
                .IsRequired(false);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.Updates)
                .WithOne(e => e.Product)
                .HasForeignKey(fk => fk.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            modelBuilder.Entity<DevelopmentTeamEntity>()
                .HasMany(e => e.Products)
                .WithOne(e => e.DevTeam)
                .HasForeignKey(fk => fk.DevTeamId)
                .IsRequired();

            modelBuilder.Entity<DevelopmentTeamEntity>()
                .HasMany(e => e.Users)
                .WithOne(e => e.DevTeam)
                .HasForeignKey(fk => fk.DevTeamId)
                .IsRequired(false);          

            #endregion

            #region M:N Relationships

            //Roles
            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<RoleEntity>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();           


            //Library Items
            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.LibraryItems)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.LibraryItems)
                .WithOne(e => e.Product)
                .HasForeignKey(pr => pr.ProductId)
                .IsRequired();

            //Reviews
            modelBuilder.Entity<ProfileEntity>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Profile)
                .HasForeignKey(fr => fr.ProfileId)
                .IsRequired(false);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Product)
                .HasForeignKey(fk => fk.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            //Bills
            modelBuilder.Entity<ProductEntity>()
                .HasMany(e => e.InvoiceItems)
                .WithOne(e => e.Product)
                .HasForeignKey(fk => fk.ProductId)
                .IsRequired();

            modelBuilder.Entity<InvoiceEntity>()
                .HasMany(e => e.CheckoutItems)
                .WithOne(e => e.Invoice)
                .HasForeignKey(fk => fk.InvoiceId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

        }
    }
}
