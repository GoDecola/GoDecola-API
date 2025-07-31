using GoDecola.API.Controllers;
using GoDecola.API.Data;
using GoDecola.API.Entities;
using GoDecola.API.Profiles;
using GoDecola.API.Repositories;
using GoDecola.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration["Jwt:SecretKey"];

var key = Encoding.ASCII.GetBytes(secretKey!);

builder.Services.AddScoped<JwtService>(
    serviceProvider =>
    {
        // injeta o UserManager<User> no JwtService
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        return new JwtService(secretKey!, userManager);
    }
);

// Configura DbContext
builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
           .EnableSensitiveDataLogging() // mostra parâmetros reais - dps remover apenas para testes
           .EnableDetailedErrors()       // mostra detalhes do erro - dps remover apenas para testes
           .LogTo(Console.WriteLine, LogLevel.Information) // joga no console - dps remover apenas para testes
    );

// Configura Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configura o JwtService
builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
)
.AddJwtBearer(
    options =>
    {
        options.RequireHttpsMetadata = false; // alterar para true em produção
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // alterar para true em produção
            ValidateAudience = false, // alterar para true em produção
        };
    }
);

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Repositories
builder.Services.AddScoped<IRepository<User, string>, UserRepository>();
builder.Services.AddScoped<IRepository<TravelPackage, int>, TravelPackageRepository>();
builder.Services.AddScoped<IRepository<Reservation, int>, ReservationRepository>();
builder.Services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();

builder.Services.AddScoped<IPaymentService>(provider =>
{
    var reservationRepo = provider.GetRequiredService<IRepository<Reservation, int>>();
    var paymentRepo = provider.GetRequiredService<IRepository<Payment, int>>();
    var logger = provider.GetRequiredService<ILogger<WebhookController>>(); // Logger para registrar eventos do Stripe, remover dps

    var stripeConfig = builder.Configuration.GetSection("Stripe");
    var stripeSettings = new StripeSettings
    {
        PublishableKey = stripeConfig["PublishableKey"] ?? throw new Exception("PublishableKey não configurado"),
        SecretKey = stripeConfig["SecretKey"] ?? throw new Exception("SecretKey não configurado")
    };

    string successUrl = stripeConfig["SuccessUrl"] ?? throw new Exception("SuccessUrl não configurado");
    string cancelUrl = stripeConfig["CancelUrl"] ?? throw new Exception("CancelUrl não configurado");

    // Aqui: retorna a instância do PaymentService, não registra outro serviço dentro
    return new PaymentService(
        reservationRepo,
        paymentRepo,
        stripeSettings,
        logger, //remover dps
        successUrl,
        cancelUrl
    );
});

// CORS
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAllOrigins",
            policy => 
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
    }
);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // converte enums para strings no JSON
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(
        sw =>
        {
            sw.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "API GoDecola",
                    Version = "v1"
                }
            );
            sw.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no campo de texto abaixo. Exemplo: Bearer {seu_token_jwt}"
                }
            );
            sw.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                }
            );
        }
);

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// SeedData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        await SeedData.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao popular o DB com as Roles: {ex.Message}");
    }

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
