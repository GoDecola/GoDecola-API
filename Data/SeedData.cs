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

            if (!await roleManager.RoleExistsAsync("ADMIN"))
            {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
            }

            if (!await roleManager.RoleExistsAsync("ATTENDANT"))
            {
                await roleManager.CreateAsync(new IdentityRole("ATTENDANT"));
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
                };

                var resultado = await userManager.CreateAsync(adminUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "ADMIN");
                }
            }

            var attendantUser = await userManager.FindByEmailAsync("attendant@godecola.com");

            if (attendantUser == null)
            {
                attendantUser = new User
                {
                    UserName = "attendant@godecola.com",
                    Email = "attendant@godecola.com",
                    FirstName = "Attendant",
                    LastName = "GoDecola",
                };

                var resultado = await userManager.CreateAsync(attendantUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(attendantUser, "ATTENDANT");
                }
            }

            if (!context.TravelPackages.Any())
            {
                context.TravelPackages.AddRange(new[]
                {
                    new TravelPackage
                    {
                        Title = "Hotel Fazenda em Socorro",
                        Description = "Desfrute de um final de semana relaxante em um hotel fazenda em Socorro, SP. Inclui café da manhã, almoço e jantar.",
                        Price = 589.0,
                        Destination = "Socorro, SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(5),
                        NumberGuests = 2,
                        NumberBaths = 1,
                        NumberBeds = 1,
                        Amenities = new HotelAmenities
                        {
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
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
                        Price = 560.0,
                        Destination = "Ubatuba, SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(3),
                        NumberGuests = 6,
                        NumberBaths = 2,
                        NumberBeds = 4,
                        Amenities = new HotelAmenities
                        {
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
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
                        Price = 2200.0,
                        Destination = "Tokyo",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(1).AddDays(7),
                        NumberGuests = 2,
                        NumberBaths = 1,
                        NumberBeds = 1,
                        Amenities = new HotelAmenities
                        {
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = false, // Exemplo de alteração
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
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
