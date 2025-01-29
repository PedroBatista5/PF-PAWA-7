using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SeuProjeto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddScoped<UtilizadorService>();
builder.Services.AddScoped<ProjetosService>();
builder.Services.AddScoped<IContratacaoService, ContratacaoService>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Adiciona autenticação JWT com chave fixa
// Chave secreta com 256 bits (32 bytes)
var secretKey = Encoding.UTF8.GetBytes("MINHA_CHAVE_SUPER_SECRETA_QUE_TEM_32_BYTES!"); // 32 caracteres
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");

app.UseRouting(); // Altere para que UseRouting venha antes de UseAuthentication/UseAuthorization
app.UseAuthentication(); // Adiciona autenticação
app.UseAuthorization();  // Adiciona autorização

app.MapControllers();

app.Run();
