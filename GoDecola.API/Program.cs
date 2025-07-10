using AutoMapper;
using GoDecola.API.Mapper;
using GoDecola.API.Service;

var builder = WebApplication.CreateBuilder(args);

// Serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>());

// Injeção de Dependência -- lembrar de pesquisar sobre Scrutor
builder.Services.AddScoped<IUserService, MockUserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();