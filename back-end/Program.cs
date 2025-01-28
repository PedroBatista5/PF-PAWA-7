using Backend.Services;
using Microsoft.EntityFrameworkCore;
using SeuProjeto.Services; // Se estiver no namespace correto

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");

app.UseRouting();
app.MapControllers(); 

app.Run();
