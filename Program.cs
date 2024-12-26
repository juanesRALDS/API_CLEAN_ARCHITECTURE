using SagaAserhi.Application.UseCases;
using SagaAserhi.Application.UseCases.Auth;
using SagaAserhi.Infrastructure.Utils;
using SagaAserhi.Application.UseCases.Users;
using SagaAserhi.Domain.Entities;
using SagaAserhi.Infrastructure.Config;
using SagaAserhi.Infrastructure.Context;
using SagaAserhi.Infrastructure.Repositories;
using SagaAserhi.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using SagaAserhi.Application.Interfaces.UseCaseUsers;
using SagaAserhi.Application.Interfaces.Auth;
using SagaAserhi.Application.Interfaces;
using SagaAserhi.Application.Interfaces.Auth.IAuthUsecases;
using SagaAserhi.Application.Interfaces.Utils;
using SagaAserhi.Application.Interfaces.UseCasePotentialClient;
using SagaAserhi.Application.UseCases.PotentialClientsUseCase;

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
builder.Services.AddScoped<IGetAllPotentialClientsWithProposalsUseCase, GetAllPotentialClientsWithProposalsUseCase>();
builder.Services.AddScoped<ICreatePotentialClientUseCase, CreatePotentialClientUseCase>();
builder.Services.AddScoped<IUpdatePotentialClientUseCase, UpdatePotentialClientUseCase>();
builder.Services.AddScoped<IDeletePotentialClientUseCase, DeletePotentialClientUseCase>();

builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();  
builder.Services.AddScoped<IPotentialClientRepository, PotentialClientRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenServices>();
builder.Services.AddScoped<IPasswordHasher,SagaAserhi.Infrastructure.Services.PasswordHasher>();


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
