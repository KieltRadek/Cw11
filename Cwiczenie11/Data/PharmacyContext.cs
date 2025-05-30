using Cwiczenie11.Data;
using Cwiczenie11.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodajemy DbContext (ConnectionString w appsettings.json pod kluczem "Default")
builder.Services.AddDbContext<PharmacyContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")
    )
);

// 2. Rejestracja serwisów
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPatientService, PatientService>();

// 3. Dodajemy obsługę kontrolerów
builder.Services.AddControllers();

// 4. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// jeśli w kontrolerach masz [Authorize]
app.UseAuthorization();

// 6. Mappujemy wszystkie kontrolery z atrybutem [ApiController]
app.MapControllers();

// (opcjonalnie) domyślny endpoint
// app.MapGet("/weatherforecast", …);

app.Run();