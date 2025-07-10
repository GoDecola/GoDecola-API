using GoDecola.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext com SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adição dos serviços de controllers
builder.Services.AddControllers();

var app = builder.Build();

// Redirecionamento HTTPS
app.UseHttpsRedirection();

// Middleware de autorização
app.UseAuthorization();

// Mapeamento dos endpoints dos controllers
app.MapControllers();

app.Run();

