using Microsoft.AspNetCore.Authentication.JwtBearer;
using MiniDropBox.Application.Injections;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using MiniDropBox.API.Swagger;
using MiniDropBox.Infraestructure.Injections;
using MiniDropBox.Infraestructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Configuration.AddUserSecrets("d8cb55f4-7146-435d-940d-c68dce9df836");
var secret = builder.Configuration["JwtSettings:JwtKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "MiniDropBoxApi",
            ValidAudience = "MiniDropBoxClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!))
        };
    });

builder.Services.AddAuthorization();

//Add infraestructure services
builder.Services.AddApplicationDependencies();

//Add application services
builder.Services.AddApplicationServices();

builder.Services.AddDatabaseProvider(builder.Configuration);

//Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Put the JWT token here",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{ }
        }
    });

    c.OperationFilter<SwaggerFileUploadOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
