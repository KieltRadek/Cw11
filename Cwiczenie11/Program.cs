using Cwiczenie11.Data;
using Cwiczenie11.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja DbContext (upewnij się, że connection string jest poprawnie skonfigurowany w appsettings.json)
builder.Services.AddDbContext<PharmacyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

// Rejestracja serwisów
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPatientService, PatientService>();

// Dodajemy kontrolery (przy REST API)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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