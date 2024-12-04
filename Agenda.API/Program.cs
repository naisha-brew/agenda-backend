using System.Security.Cryptography;
using System.Text;
using Agenda.API.Database;
using Agenda.API.interfaces;
using Agenda.API.models;
using Agenda.API.repository;
using Agenda.API.services;
using Agenda.API.Swagger;
using Agenda.API.validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAgendaRepository, AgendaRepository>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<AgendaService>();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSingleton<IDbConnectionFactory>(_ => 
    new NpgsqlDbConnectionFactory(builder.Configuration["DbConnectionString"]!));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserModelValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AgendaModelValidator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permite cualquier origen
            .AllowAnyMethod() // Permite cualquier método (GET, POST, etc.)
            .AllowAnyHeader(); // Permite cualquier cabecera
    });
    options.AddPolicy("AllowMiFrontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:3000") // Cambia al origen permitido
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = false;
    options.UseSecurityTokenValidators = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = "JwtIssuer",
        ValidAudience = "JwtAudience",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding
            .UTF8.GetBytes(builder.Configuration["Jwt:KeySecret"]!))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
