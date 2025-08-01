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
                };

                var resultado = await userManager.CreateAsync(supportUser, "GoDecola@123");

                if (resultado.Succeeded)
                {
                    await userManager.AddToRoleAsync(supportUser, UserType.SUPPORT.ToString());
                }
            }

            if (!context.TravelPackages.Any())
            {
                var addressUbatuba = new Address
                {
                    AddressLine1 = "Rua da Praia",
                    AddressLine2 = "123",
                    City = "Ubatuba",
                    State = "São Paulo",
                    ZipCode = "13960-000",
                    Country = "Brasil",
                    Neighborhood = "Praia Grande",
                    Latitude = -23.4335,
                    Longitude = -45.0833
                };

                var addressChale = new Address
                {
                    AddressLine1 = "Rua das Flores",
                    AddressLine2 = "456",
                    City = "Campos do Jordão",
                    State = "São Paulo",
                    ZipCode = "11680-000",
                    Country = "Brasil",
                    Neighborhood = "Capivari",
                    Latitude = -22.7383,
                    Longitude = -45.5920
                };

                var addressFlat = new Address
                {
                    AddressLine1 = "Avenida Paulista",
                    AddressLine2 = "789",
                    City = "São Paulo",
                    State = "São Paulo",
                    ZipCode = "11680-000",
                    Country = "Brasil",
                    Neighborhood = "Bela Vista",
                    Latitude = -23.5505,
                    Longitude = -46.6333
                };

                var addressPousada = new Address
                {
                    AddressLine1 = "Rua das Flores",
                    AddressLine2 = "456",
                    City = "Paraty",
                    State = "Rio de Janeiro",
                    ZipCode = "11680-000",
                    Country = "Brasil",
                    Neighborhood = "Centro Histórico",
                    Latitude = -23.2221,
                    Longitude = -44.7176
                };

                var addressHotel = new Address
                {
                    AddressLine1 = "Rua do Campo",
                    AddressLine2 = "123",
                    City = "Socorro",
                    State = "São Paulo",
                    ZipCode = "11680-000",
                    Country = "Brasil",
                    Neighborhood = "Centro",
                    Latitude = -22.5903,
                    Longitude = -46.5251
                };

                var addressCamping = new Address
                {
                    AddressLine1 = "Rua do Campo",
                    AddressLine2 = "738",
                    City = "Bonito",
                    State = "Mato Grosso do Sul",
                    ZipCode = "13960-000",
                    Country = "Brasil",
                    Neighborhood = "Centro",
                    Latitude = -21.1261,
                    Longitude = -56.4844
                };

                var addressApartamento = new Address
                {
                    AddressLine1 = "Avenida Atlântica",
                    AddressLine2 = "456",
                    City = "Florianópolis",
                    State = "Santa Catarina",
                    ZipCode = "13960-000",
                    Country = "Brasil",
                    Neighborhood = "Centro",
                    Latitude = -27.5954,
                    Longitude = -48.5480
                };

                var addressHostel = new Address
                {
                    AddressLine1 = "Rua do Ouro",
                    AddressLine2 = "789",
                    City = "Ouro Preto",
                    State = "Minas Gerais",
                    ZipCode = "35400-000",
                    Country = "Brasil",
                    Neighborhood = "Centro Histórico",
                    Latitude = -20.3850,
                    Longitude = -43.5038
                };

                var addressSitio = new Address
                {
                    AddressLine1 = "Rua do Sítio",
                    AddressLine2 = "123",
                    City = "Atibaia",
                    State = "Minas Gerais",
                    ZipCode = "12940-000",
                    Country = "Brasil",
                    Neighborhood = "Centro",
                    Latitude = -23.1171,
                    Longitude = -46.5563
                };

                var addressSuite = new Address
                {
                    AddressLine1 = "Avenida Beira Mar",
                    AddressLine2 = "4438",
                    City = "Natal",
                    State = "Rio Grande do Norte",
                    ZipCode = "59090-001",
                    Country = "Brasil",
                    Neighborhood = "Ponta Negra",
                    Latitude = -5.7945,
                    Longitude = -35.2110
                };

                var addressParis = new Address
                {
                    AddressLine1 = "Avenue des Champs-Élysées",
                    AddressLine2 = "123",
                    City = "Paris",
                    State = "Île-de-France",
                    ZipCode = "75008",
                    Country = "França",
                    Neighborhood = "Champs-Élysées",
                    Latitude = 48.8566,
                    Longitude = 2.3522
                };

                var addressMexico = new Address
                {
                    AddressLine1 = "Boulevard Kukulcan",
                    AddressLine2 = "789",
                    City = "Cancún",
                    State = "Quintana Roo",
                    ZipCode = "77500",
                    Country = "México",
                    Neighborhood = "Zona Hotelera",
                    Latitude = 21.1619,
                    Longitude = -86.8515
                };

                var addressYork = new Address
                {
                    AddressLine1 = "5th Avenue",
                    AddressLine2 = "101",
                    City = "Nova York",
                    State = "Nova York",
                    ZipCode = "10010",
                    Country = "EUA",
                    Neighborhood = "Manhattan",
                    Latitude = 40.7128,
                    Longitude = -74.0060
                };

                var addressIguacu = new Address
                {
                    AddressLine1 = "Avenida das Cataratas",
                    AddressLine2 = "456",
                    City = "Puerto Iguazu",
                    State = "Misiones",
                    ZipCode = "85855-750",
                    Country = "Argentina",
                    Neighborhood = "Centro",
                    Latitude = -25.5972,
                    Longitude = -54.5781
                };

                var addressBali = new Address
                {
                    AddressLine1 = "Jalan Raya Ubud",
                    AddressLine2 = "123",
                    City = "Ubud",
                    State = "Bali",
                    ZipCode = "80571",
                    Country = "Indonésia",
                    Neighborhood = "Centro",
                    Latitude = -8.3405,
                    Longitude = 115.0920
                };

                var addressTailandia = new Address
                {
                    AddressLine1 = "Sukhumvit Road",
                    AddressLine2 = "456",
                    City = "Bangkok",
                    State = "Bangkok",
                    ZipCode = "10110",
                    Country = "Tailândia",
                    Neighborhood = "Sukhumvit",
                    Latitude = 13.7563,
                    Longitude = 100.5018
                };

                var addressItalia = new Address
                {
                    AddressLine1 = "Via Roma",
                    AddressLine2 = "123",
                    City = "Roma",
                    State = "Lazio",
                    ZipCode = "00100",
                    Country = "Itália",
                    Neighborhood = "Centro Histórico",
                    Latitude = 41.9028,
                    Longitude = 12.4964
                };

                var addressDubai = new Address
                {
                    AddressLine1 = "Deserto de Dubai",
                    AddressLine2 = "246",
                    City = "Dubai",
                    State = "Dubai",
                    ZipCode = "05562",
                    Country = "Emirados Árabes Unidos",
                    Neighborhood = "Deserto",
                    Latitude = 25.2048,
                    Longitude = 55.2708
                };

                var addressTokyo = new Address
                {
                    AddressLine1 = "Rua da Cultura",
                    AddressLine2 = "456",
                    City = "Tokyo",
                    State = "Tokyo",
                    ZipCode = "12345-678",
                    Country = "Japão",
                    Neighborhood = "Shibuya",
                    Latitude = 35.6895,
                    Longitude = 139.6917
                };

                var addressCalifornia = new Address
                {
                    AddressLine1 = "Pacific Coast Highway",
                    AddressLine2 = "46",
                    City = "Monterey",
                    State = "Califórnia",
                    ZipCode = "93923",
                    Country = "Estados Unidos",
                    Neighborhood = "Carmel-by-the-Sea",
                    Latitude = 36.3615,
                    Longitude = -121.8563
                };

                context.TravelPackages.AddRange(new[]
                {
                    new TravelPackage //id 1
                    {
                        Title = "Casa na Praia em Ubatuba",
                        Description = "Desfrute de dias incríveis em uma casa aconchegante, a poucos metros da Praia Grande em Ubatuba/SP. Ideal para casais, com piscina, Wi-Fi, ar-condicionado e um ambiente pet friendly. O refúgio perfeito para relaxar com conforto e praticidade.",
                        Price = (long)(589.0 * 100),
                        Destination = "Ubatuba/Sp",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(50),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 2,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressUbatuba,
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_01A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 2
                    {

                        Title = "Chalé Aconchegante em Campos do Jordão",
                        Description = "Experimente o charme da serra em um chalé intimista em Campos do Jordão. Perfeito para uma escapada romântica com lareira, conforto e fácil acesso ao centro turístico da cidade. Um refúgio tranquilo em meio às montanhas.",
                        Price = (long)(450.0 * 100),
                        Destination = "Campos do Jordão/Sp",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(80),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        DiscountPercentage = 0.15, // 15% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-2), // promoção começou 2 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(5), // promoção termina em 5 dias
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
                            HasPetFriendly = true,
                            Address = addressChale
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_02A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 3
                    {
                        Title = "Flat no Centro de São Paulo",
                        Description = "Hospede-se no coração de São Paulo com todo o conforto de um flat completo. Próximo à Av. Paulista, com academia, piscina, café da manhã incluso e ideal para quem busca localização estratégica e estrutura moderna",
                        Price = (long)(320.0 * 100),
                        Destination = "São Paulo/Sp",
                        StartDate = DateTime.Now.AddMonths(1),
                        EndDate = DateTime.Now.AddMonths(1).AddDays(7),
                        IsActive = true,
                        NumberGuests = 2,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true, // Exemplo de alteração
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = false,
                            HasPetFriendly = true,
                            Address = addressFlat
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_03A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 4
                    {

                        Title = "Pousada Charmosa em Paraty",
                        Description = "Relaxe em uma pousada encantadora próxima às praias e ao centro histórico de Paraty/RJ. Com piscina, restaurante e estrutura para toda a família, este é o destino ideal para quem busca cultura, natureza e charme colonial.",
                        Price = (long)(390.0 * 100),
                        Destination = "Rio de Janeiro/RJ",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(45),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        DiscountPercentage = 0.10, // 10% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-5), // promoção começou 2 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(3), // promoção termina em 5 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressPousada
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_04A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 5
                    {

                        Title = "Hotel Fazenda em Socorro",
                        Description = "Curta o melhor do interior paulista em um hotel fazenda acolhedor em Socorro/SP. Cercado por natureza, com piscina, restaurante e atividades para todas as idades, é perfeito para quem deseja fugir da rotina em meio ao verde.",
                        Price = (long)(420.0 * 100),
                        Destination = "Socorro/SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(90),
                        NumberGuests = 3,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        DiscountPercentage = 0.08, // 8% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-3), // promoção começou 3 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(5), // promoção termina em 5 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressHotel
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_05A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 6
                    {

                        Title = "Camping Luxo em Bonito - MS",
                        Description = "Para os amantes da natureza com conforto, esse camping em Bonito oferece uma experiência única! Aproveite a infraestrutura moderna, localização privilegiada e viva dias de aventura cercado pelas belezas naturais do Mato Grosso do Sul.",
                        Price = (long)(280.0 * 100),
                        Destination = "Bonito/MS",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(30),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = false,
                            Address = addressCamping
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_06A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 7
                    {

                        Title = "Apartamento de frente para o mar em Floripa",
                        Description = "Acorde com o som do mar neste apartamento moderno em Florianópolis/SC. Com vista panorâmica, piscina, academia e fácil acesso às principais praias da ilha, é ideal para quem busca sofisticação e contato com a natureza.",
                        Price = (long)(650.0 * 100),
                        Destination = "Floripa/SC",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(180),
                        NumberGuests = 5,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressApartamento
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_07A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 8
                    {

                        Title = "Hostel Estilo Europeu em Ouro Preto",
                        Description = "Sinta-se na Europa em pleno Brasil! Este hostel charmoso em Ouro Preto/MG oferece conforto, gastronomia local e uma arquitetura que remete ao velho continente. Uma opção econômica e elegante no coração histórico de Minas Gerais.",
                        Price = (long)(200.0 * 100),
                        Destination = "Ouro Preto/MG",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(288),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 2,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressHostel
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_08A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 9
                    {

                        Title = "Sítio com Piscina em Atibaia",
                        Description = "Ideal para famílias e grupos, este sítio em Atibaia/SP conta com piscina, áreas verdes e ambiente acolhedor para momentos inesquecíveis com quem você ama. A poucos quilômetros de São Paulo, oferece o descanso que você procura.",
                        Price = (long)(700.0 * 100),
                        Destination = "Socorro/SP",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(155),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        DiscountPercentage = 0.5, // 5% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-5), // promoção começou 5 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(3), // promoção termina em 3 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 3,
                            NumberBeds = 4,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressSitio
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_09A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 10
                    {

                        Title = "Suíte Master em resort em Natal",
                        Description = "Descubra o paraíso nordestino com conforto premium! Esta suíte master em resort em Natal/RN oferece café da manhã, piscina, ar-condicionado e localização privilegiada à beira-mar. Uma experiência única na Cidade do Sol.",
                        Price = (long)(890.0 * 100),
                        Destination = "Natal/RN",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(365),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.NATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressSuite
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_10A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 11
                    {

                        Title = "Pacote Paris - França",
                        Description = "Viva o romantismo da Cidade Luz! Este pacote inclui hospedagem com ar-condicionado, café da manhã e fácil acesso aos principais pontos turísticos como a Torre Eiffel, Champs-Élysées e o Louvre. Ideal para uma viagem inesquecível a dois.",
                        Price = (long)(1200.0 * 100),
                        Destination = "Paris, França",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(25),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressParis
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_11A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 12
                    {

                        Title = "Viagem para Cancún - México",
                        Description = "Aproveite as águas cristalinas do Caribe com este pacote exclusivo para Cancún. Hospedagem completa com estrutura para relaxar, restaurante e localização próxima às praias paradisíacas. Um paraíso tropical com muito conforto.",
                        Price = (long)(1400.0 * 100),
                        Destination = "Cancún, México",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(120),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        DiscountPercentage = 0.20, // 20% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-3), // promoção começou 3 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(4), // promoção termina em 4 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 1,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressMexico
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_12A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 13
                    {

                        Title = "Explorando Nova York - EUA",
                        Description = "Descubra a energia única da Big Apple! Com hospedagem bem localizada, estrutura completa e café da manhã incluso, você poderá explorar Manhattan, a Times Square e Central Park com estilo e conforto.",
                        Price = (long)(1800.0 * 100),
                        Destination = "Nova York, EUA",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(50),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 2,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressYork
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_13A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 14
                    {

                        Title = "Aventura nas Cataratas do Iguaçu - Argentina",
                        Description = "Mergulhe na natureza com este pacote para as Cataratas do Iguaçu, lado argentino. Ideal para aventureiros e apaixonados por paisagens exuberantes, com hospedagem próxima ao parque NATIONAL e restaurante típico incluso.",
                        Price = (long)(900.0 * 100),
                        Destination = "Cataratas do Iguaçu, Argentina",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(30),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        DiscountPercentage = 0.15, // 15% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-5), // promoção começou 2 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(3), // promoção termina em 5 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 2,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressIguacu
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_14A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 15
                    {

                        Title = "Lua de mel em Bali - Indonésia",
                        Description = "Celebre o amor em um dos destinos mais românticos do mundo! Este pacote para Bali oferece luxo, paisagens de tirar o fôlego, piscina e estrutura ideal para casais. Um verdadeiro paraíso para lua de mel inesquecível.",
                        Price = (long)(2100.0 * 100),
                        Destination = "Bali, Indonésia",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(15),
                        NumberGuests = 2,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressBali
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_15A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 16
                    {

                        Title = "Tour pela Tailândia",
                        Description = "Explore templos exóticos, praias deslumbrantes e cultura vibrante neste tour completo pela Tailândia. Com hospedagem confortável e café da manhã incluso, prepare-se para uma jornada mágica no sudeste asiático.",
                        Price = (long)(1300.0 * 100),
                        Destination = "Tailândia",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(125),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = false,
                            HasGym = false,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressTailandia
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_16A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 17
                    {

                        Title = "Férias em Roma - Itália",
                        Description = "Descubra o melhor da história, arte e gastronomia em Roma! Este pacote oferece acomodação no centro da capital italiana, perfeita para explorar o Coliseu, o Vaticano e degustar a autêntica culinária italiana.",
                        Price = (long)(1600.0 * 100),
                        Destination = "Roma, Itália",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(5),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressItalia
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_17A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 18
                    {

                        Title = "Deserto e luxo em Dubai",
                        Description = "Conheça uma das cidades mais famosas do mundo e em luxo.",
                        Price = (long)(2500.0 * 100),
                        Destination = "Dubai, Emirados Árabes Unidos",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(15),
                        NumberGuests = 5,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 2,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressDubai
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_18A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 19
                    {

                        Title = "Cultura e gastronomia no Japão",
                        Description = "Uma das Gastronomia mais ricas no mundo.",
                        Price = (long)(2200.0 * 100),
                        Destination = "Japão",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(35),
                        NumberGuests = 5,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        DiscountPercentage = 0.10, // 10% de desconto
                        PromotionStartDate = DateTime.Now.AddDays(-1), // promoção começou 1 dias atrás
                        PromotionEndDate = DateTime.Now.AddDays(6), // promoção termina em 6 dias
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 1,
                            NumberBeds = 3,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressTokyo
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_19A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                    new TravelPackage // id 20
                    {

                        Title = "Road trip pela Califórnia - EUA",
                        Description = "Uma trip incrível te aguarda na Califórnia - EUA.",
                        Price = (long)(1700.0 * 100),
                        Destination = "Califórnia - EUA",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(22),
                        NumberGuests = 4,
                        IsActive = true,
                        PackageType = PackageType.INTERNATIONAL,
                        AccommodationDetails = new AccommodationDetails
                        {
                            NumberBaths = 3,
                            NumberBeds = 2,
                            HasAirConditioning = true,
                            HasWifi = true,
                            HasParking = true,
                            HasPool = true,
                            HasGym = true,
                            HasRestaurant = true,
                            HasBreakfastIncluded = true,
                            HasPetFriendly = true,
                            Address = addressCalifornia
                        },
                        Medias = new List<TravelPackageMedia>
                        {
                            new TravelPackageMedia { FilePath = "/Medias/Images/TravelPackage/packcageld_20A.jpg", UploadDate = DateTime.UtcNow, MimeType = "image/jpeg" },
                        }
                    },
                });
            }

            await context.SaveChangesAsync();
        }
    }
}
