using FirstAPINet;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mysqlConnection = builder.Configuration.GetConnectionString("MySQLConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseMySql(mysqlConnection, ServerVersion.AutoDetect(mysqlConnection)));

// Configurar servicios para controlar las referencias circulares
/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });*/

// Agregar servicios CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()   // Permite cualquier origen
               .AllowAnyMethod()   // Permite cualquier método (GET, POST, etc.)
               .AllowAnyHeader()); // Permite cualquier encabezado
});


var app = builder.Build();

// Asegúrate de colocar `UseCors` antes de cualquier otro middleware
app.UseCors("AllowAll");  // Aplica la política CORS

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
