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
using Stripe;

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
builder.Services.AddScoped<IRepository<Payment, int>, PaymentRepository>();
builder.Services.AddScoped<ReservationRepository>();

// Services
builder.Services.AddScoped<IPaymentService>(provider =>
{
    var reservationRepo = provider.GetRequiredService<IRepository<Reservation, int>>();
    var paymentRepo = provider.GetRequiredService<IRepository<Payment, int>>();

    var stripeOptions = builder.Configuration.GetSection("Stripe").Get<StripeSettings>();

    // URLs para redirecionamento após o pagamento com Stripe:
    // - SuccessUrl: para onde o usuário será enviado após pagamento sucedido
    // - CancelUrl: para onde o usuário será enviado caso cancele o pagamento
    var successUrl = builder.Configuration["Stripe:SuccessUrl"];
    var cancelUrl = builder.Configuration["Stripe:CancelUrl"];

    return new PaymentService(reservationRepo, paymentRepo, stripeOptions, successUrl, cancelUrl);
});

// Configura Stripe
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"]; // Configura a chave secreta da Stripe

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

app.UseAuthorization();

app.MapControllers();

app.Run();
