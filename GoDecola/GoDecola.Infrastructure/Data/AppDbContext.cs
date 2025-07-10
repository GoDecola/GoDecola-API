using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoDecola.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<PackageDateRange> PackageDateRanges { get; set; }
        public DbSet<PackageMedia> PackageMedias { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TravelPackage>()
             .Property(p => p.BasePrice)
             .HasPrecision(10, 2);

            modelBuilder.Entity<Reservation>()
            .Property(r => r.TotalAmount)
            .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(10, 2);

            modelBuilder.Entity<PackageDateRange>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}

