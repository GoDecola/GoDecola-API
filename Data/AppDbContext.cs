using GoDecola.API.Entities;
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
        public DbSet<HotelAmenities> HotelAmenities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Guests> Guests { get; set; }

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
                .HasMany(tp => tp.Images) // um pacote de viagem pode ter várias imagens
                .WithOne(tpi => tpi.TravelPackage) // Propriedade de navegação inversa
                .HasForeignKey(tpi => tpi.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Cascade); // se o pacote for excluído, as imagens também serão excluídas

            builder.Entity<TravelPackage>()
                .HasMany(tp => tp.Videos) // um pacote de viagem pode ter vários vídeos
                .WithOne(tpv => tpv.TravelPackage) // Propriedade de navegação inversa
                .HasForeignKey(tpv => tpv.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Cascade); // se o pacote for excluído, os vídeos também serão excluídos

            builder.Entity<TravelPackage>()
                .HasOne(tp => tp.Amenities) // Configura a relação de um para um com HotelAmenities
                .WithOne() // Propriedade de navegação inversa
                .HasForeignKey<HotelAmenities>(ha => ha.TravelPackageId) // fk
                .IsRequired();

            // ------------------- RESERVATIONS ----------------------

            builder.Entity<Reservation>()
                .HasOne(r => r.User) // uma reserva tem 01 usuário
                .WithMany() // 01 usuario pode ter várias reservas
                .HasForeignKey(r => r.UserId) // fk
                .OnDelete(DeleteBehavior.Restrict); // não exclui usuário se houver reservas

            builder.Entity<Reservation>()
                .HasOne(r => r.TravelPackage) // uma reserva tem 01 pacote de viagem
                .WithMany() // 01 pacote de viagem pode ter várias reservas
                .HasForeignKey(r => r.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Restrict); // não exclui pacote de viagem se houver reservas

            builder.Entity<Reservation>()
                .HasMany(r => r.Guests) // uma reserva pode ter vários hóspedes
                .WithOne()
                .HasForeignKey(g => g.ReservationId) // fk
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
