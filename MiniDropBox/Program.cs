using Microsoft.AspNetCore.Authentication.JwtBearer;
using MiniDropBox.Application.Injections;
using Microsoft.IdentityModel.Tokens;
using MiniDropBox.Config;
using System.Text;

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

//Add infraestructure services
builder.Services.AddApplicationDependencies();

//Add application services
builder.Services.AddApplicationServices();

//Add swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
