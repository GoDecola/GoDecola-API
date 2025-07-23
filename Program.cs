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
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration["Jwt:SecretKey"];

var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddScoped<JwtService>(
    serviceProvider =>
    {
        // injeta o UserManager<User> no JwtService
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        return new JwtService(secretKey, userManager);
    }
);

// Configura DbContext
builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
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
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
