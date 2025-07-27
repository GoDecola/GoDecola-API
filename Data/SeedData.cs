using GoDecola.API.Entities;
using GoDecola.API.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoDecola.API.Data
{
    public class SeedData
    {
        public static async Task Initialize(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!await roleManager.RoleExistsAsync(UserType.ADMIN.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(UserType.ADMIN.ToString()));
            }

            if (!await roleManager.RoleExistsAsync(UserType.SUPPORT.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(UserType.SUPPORT.ToString()));
            }

            if (!await roleManager.RoleExistsAsync(UserType.USER.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(UserType.USER.ToString()));
            }

            var adminUser = await userManager.FindByEmailAsync("admin@godecola.com");

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "admin@godecola.com",
                    Email = "admin@godecola.com",
                    FirstName = "Admin",
                    LastName = "GoDecola",
                    Type = UserType.ADMIN,
                };

                var resultado = await userManager.CreateAsync(adminUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserType.ADMIN.ToString());
                }
            }

            var supportUser = await userManager.FindByEmailAsync("support@godecola.com");

            if (supportUser == null)
            {
                supportUser = new User
                {
                    UserName = "support@godecola.com",
                    Email = "support@godecola.com",
                    FirstName = "Support",
                    LastName = "GoDecola",
                    Type = UserType.SUPPORT,
                };

                var resultado = await userManager.CreateAsync(supportUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(supportUser, UserType.SUPPORT.ToString());
                }
            }

            if (!context.TravelPackages.Any())
            {
                var addressSocorro = new Address
                {
                    AddressLine1 = "Rua das Flores, 123",
                    City = "Socorro",
                    State = "SP",
                    ZipCode = "13960-000",
                    Country = "Brasil",
                    Neighborhood = "Centro",
                    Latitude = -22.5937,
                    Longitude = -46.5684
                };

                var addressUbatuba = new Address
                {
                    AddressLine1 = "Avenida da Praia, 456",
                    City = "Ubatuba",
                    State = "SP",
                    ZipCode = "11680-000",
                    Country = "Brasil",
                    Neighborhood = "Itaguá",
                    Latitude = -23.4333,
                    Longitude = -45.0833
                };

                var addressTokyo = new Address
                {
                    AddressLine1 = "Shinjuku, Tokyo",
                    City = "Tokyo",
                    State = "Tokyo",
                    ZipCode = "160-0022",
                    Country = "Japão",
                    Neighborhood = "Shinjuku",
                    Latitude = 35.6895,
                    Longitude = 139.6917
                };

                context.TravelPackages.AddRange(new[]
                {
                    new TravelPackage
                    {
                        Title = "Hotel Fazenda em Socorro",
                        Description = "Desfrute de um final de semana relaxante em um hotel fazenda em Socorro, SP. Inclui café da manhã, almoço e jantar.",
                        Price = (long)(589.0 * 100),
                        Destination = "Socorro, SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(5),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.National,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
                            Address = addressSocorro,
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { MediaUrl = "https://hansenimoveis.com/wp-content/uploads/2022/02/CO653-3.jpg", UploadDate = DateTime.UtcNow, MediaType = MediaType.Image },
                        } 
                    },
                    new TravelPackage 
                    { 

                        Title = "Casa de Praia em Ubatuba",
                        Description = "Bonita casa de praia em Ubatuba",
                        Price = (long)(560.0 * 100),
                        Destination = "Ubatuba, SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(3),
                        NumberGuests = 6,
                        IsActive = true,
                        PackageType = PackageType.National,
                        DiscountPercentage = 0.15, // 15% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-2), // promoção começou 2 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(5), // promoção termina em 5 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 4,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
                            Address = addressUbatuba
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { MediaUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSacQlg6kYF4qTkWf80HRLnhvHSt6bvIWho8A&s", UploadDate = DateTime.UtcNow, MediaType = MediaType.Image },
                        }
                    },
                    new TravelPackage
                    {
                        Title = "Cultura e gastronomia no Japão",
                        Description = "Passeio por Tokyo",
                        Price = (long)(2200.0 * 100),
                        Destination = "Tokyo",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(1).AddDays(7),
                        IsActive = true,
                        NumberGuests = 2,
                        PackageType = PackageType.International,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = false, // Exemplo de alteração
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
                            Address = addressTokyo
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { MediaUrl = "https://thumbs.dreamstime.com/b/vida-noturna-em-dotonbori-osaka-kansai-japan-267022014.jpg", UploadDate = DateTime.UtcNow, MediaType = MediaType.Image },
                        }
                    }
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
