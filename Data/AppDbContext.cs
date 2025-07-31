using GoDecola.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<TravelPackage> TravelPackages { get; set; }
        public DbSet<TravelPackageMedia> TravelPackageMedias { get; set; }
        public DbSet<AccommodationDetails> AccommodationDetails { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Payment> Payments { get; set; }

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
                .Property(p => p.PackageType) // configura o package type como string
                .HasConversion<string>();

            builder.Entity<TravelPackage>()
                .HasMany(tp => tp.Medias) // um pacote de viagem pode ter várias mídias
                .WithOne(tpm => tpm.TravelPackage) // Propriedade de navegação inversa
                .HasForeignKey(tpm => tpm.TravelPackageId) // fk
                .OnDelete(DeleteBehavior.Cascade); // exclui mídias se o pacote for excluído

            builder.Entity<TravelPackage>()
                .HasOne(tp => tp.AccommodationDetails) // Configura a relação de um para um com AccommodationDetails
                .WithOne() // Propriedade de navegação inversa
                .HasForeignKey<AccommodationDetails>(ha => ha.TravelPackageId) // fk
                .IsRequired();

            builder.Entity<AccommodationDetails>()
                .HasOne(ad => ad.Address) // uma acomodação tem 01 endereço
                .WithOne() // 01 endereço pode ter apenas uma acomodação
                .HasForeignKey<AccommodationDetails>(ad => ad.AddressId) // fk
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

            // ------------------- PAYMENTS ----------------------

            builder.Entity<Payment>()
                .HasOne(p => p.Reservation) // um pagamento tem 01 reserva
                .WithMany() // 01 reserva pode ter vários pagamentos
                .HasForeignKey(p => p.ReservationId) // fk
                .OnDelete(DeleteBehavior.Restrict); // não exclui reserva se houver pagamentos

        }
    }
}
