using GoDecola.API.Entities.Reservation;
using GoDecola.API.Entities.TravelPackage;
using GoDecola.API.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<TravelPackageImage> TravelPackageImages { get; set; }
        public DbSet<TravelPackageVideo> TravelPackageVideos { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<HotelAmenities> HotelAmenities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");

            // ------------------- TRAVEL PACKAGES -------------------

            builder.Entity<TravelPackage>()
                .HasMany(tp => tp.Images) // Configura a relação de um para muitos com TravelPackageImage
                .WithOne(tpi => tpi.TravelPackage) // Propriedade de navegação inversa
                .HasForeignKey(tpi => tpi.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<TravelPackage>()
                .HasMany(tp => tp.Videos) // Configura a relação de um para muitos com TravelPackageVideo
                .WithOne(tpv => tpv.TravelPackage) // Propriedade de navegação inversa
                .HasForeignKey(tpv => tpv.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Cascade);

            // ------------------- RESERVATIONS ----------------------

            builder.Entity<Reservation>()
                .HasOne<User>()
                .WithMany() // usuario pode ter várias reservas
                .HasForeignKey(r => r.UserId) // fk
                .OnDelete(DeleteBehavior.Restrict); // não exclui usuário se houver reservas

            builder.Entity<Reservation>()
                .HasOne<TravelPackage>()
                .WithMany() // pacote de viagem pode ter várias reservas
                .HasForeignKey(r => r.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Restrict); // não exclui pacote de viagem se houver reservas

            builder.Entity<Reservation>()
                .HasMany<Guests>()
                .WithOne()
                .HasForeignKey(g => g.ReservationId) // fk
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
