using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Repositories;
using api_completa_mongodb_net_6_0.Models;
using api_completa_mongodb_net_6_0.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Cargar la configuración de MongoDB desde appsettings.json
var mongoSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();

// Validar que las configuraciones no sean nulas
if (mongoSettings == null || string.IsNullOrEmpty(mongoSettings.ConnectionString))
{
    throw new ArgumentNullException("La cadena de conexión no puede ser nula.");
}

if (string.IsNullOrEmpty(mongoSettings.DatabaseName))
{
    throw new ArgumentNullException("El nombre de la base de datos no puede ser nulo.");
}

if (string.IsNullOrEmpty(mongoSettings.CollectionName))
{
    throw new ArgumentNullException("El nombre de la colección no puede ser nulo.");
}

// Registrar la configuración de MongoDB
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));

// Registrar el cliente Mongo como un servicio singleton con la cadena de conexión
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));

// Registrar la base de datos como servicio
builder.Services.AddScoped(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoSettings.DatabaseName);
});

// Registrar servicios y repositorios
builder.Services.AddSingleton<IEncryptionServices, AesOperationService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserServices>();

// Agregar controladores
builder.Services.AddControllers();

var app = builder.Build();

// Configurar los endpoints de los controladores
app.MapControllers();
app.Run();
