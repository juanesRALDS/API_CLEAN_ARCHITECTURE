using api_completa_mongodb_net_6_0.Application.UseCases;
using api_completa_mongodb_net_6_0.Application.UseCases.Auth;
using api_completa_mongodb_net_6_0.Infrastructure.Utils;
using api_completa_mongodb_net_6_0.Application.UseCases.Users;
using api_completa_mongodb_net_6_0.Domain.Entities;
using api_completa_mongodb_net_6_0.Domain.Interfaces;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Auth.IAuthUsecases;
using api_completa_mongodb_net_6_0.Infrastructure.Config;
using api_completa_mongodb_net_6_0.Infrastructure.Context;
using api_completa_mongodb_net_6_0.Infrastructure.Repositories;
using api_completa_mongodb_net_6_0.Infrastructure.Services;
using api_completa_mongodb_net_6_0.MongoApiDemo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using api_completa_mongodb_net_6_0.Domain.Interfaces.Utils;
using api_completa_mongodb_net_6_0.Domain.Interfaces.UseCaseUsers;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// **1. Configuración de MongoDB**
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddScoped<MongoDbContext>();
builder.Services.AddScoped<IMongoCollection<User>>(sp =>
{
    MongoDBSettings? settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    MongoClient? client = new MongoClient(settings.ConnectionString);
    IMongoDatabase? database = client.GetDatabase(settings.DatabaseName);
    return database.GetCollection<User>(settings.CollectionName);
});

// **2. Configuración de JwtConfig desde appsettings.json**
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<JwtConfig>>().Value);
    

// **3. Registro de dependencias**  
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IGetUserByTokenUseCase,GetUserByTokenUseCase>();
builder.Services.AddScoped<IGetAllUsersUseCase,GetAllUsersUseCase>();
builder.Services.AddScoped<IUpdateUserUseCase,UpdateUserUseCase>();
builder.Services.AddScoped<IDeleteUserUseCase,DeleteUserUseCase>();
builder.Services.AddScoped<IGetUserByIdUseCase,GetUserByIdUseCase>();
builder.Services.AddScoped<ILoginUseCase, LoginUserUseCase>();
builder.Services.AddScoped<IRegisterUseCase, RegisterUseCase>();
builder.Services.AddScoped<IUpdatePasswordUseCase,UpdatePasswordUseCase>();
builder.Services.AddScoped<IGeneratePasswordResetTokenUseCase,GeneratePasswordResetTokenUseCase>();

builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();  

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IPasswordHasher,api_completa_mongodb_net_6_0.Infrastructure.Services.PasswordHasher>();


// **4. Configuración de JWT Authentication**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        JwtConfig? jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtConfig>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.SecretKey!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience
        };
    });

// **5. Agregar controladores y Swagger**
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication? app = builder.Build();

// **6. Middlewares**
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
