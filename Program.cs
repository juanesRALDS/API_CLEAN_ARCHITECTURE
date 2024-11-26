using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using api_completa_mongodb_net_6_0.Infrastructure.Repositories;
using api_completa_mongodb_net_6_0.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoApiDemo.Infrastructure;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<EmailService>();


builder.Services.AddScoped<MongoDbContext>();


builder.Services.AddScoped<IMongoCollection<User>>(sp =>
{
    MongoDBSettings? settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    MongoClient? client = new(settings.ConnectionString);
    IMongoDatabase? database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<User>(settings.CollectionName);
});

builder.Services.AddScoped<IUserRepository, UserRepository>();





builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("TuClaveSecretaSuperSegura")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers();

app.Run();
