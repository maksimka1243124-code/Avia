using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.Data;
using System.IO.Compression;
using System.Security.Claims;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data.Common;
using FluentValidation.AspNetCore;
using Avia.Interfaces;
using Avia.IpAdress;
using Avia.Middleware;
using Avia.Middleware2;
using SQLitePCL;
using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Avia.Models;
using Avia.Data;
using Azure.Core.Pipeline;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using static Avia.Data.AddDb;
using Avia.Middleware;
using Avia.Hz;
using Avia.code;
using Avia.User.Online;
using avia.check.tickets;
using Avia.Pa;
using Avia.Middleware.Reflection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AddDb>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator<Usered>>();
builder.Services.AddOptions<AddDb.JwtOptions>()
    .Bind(builder.Configuration.GetSection(AddDb.JwtOptions.name))
    .ValidateDataAnnotations()
    .ValidateOnStart();
var secretKey = builder.Configuration["Jwt:Key"]; 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
var app = builder.Build();
app.UseAuthentication();
app.UseMiddleware<Iticketservice>();
app.UseMiddleware<Check>();
app.UseMiddleware<TransMiddleware>();
app.UseMiddleware<Checker>();
app.UseMiddleware<Iticketservice>();
app.UseMiddleware<Pagination>();
app.UseMiddleware<IpLog>();
app.UseMiddleware<Middleware>();
app.UseMiddleware<Reflection>();
app.UseMiddleware<Online>();

app.UseAuthorization();
app.MapControllers();
string GenerateJwtToken(string userId,string role,string secretkey)
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
    var code = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        expires: DateTime.Now.AddHours(1),
     signingCredentials: code,
      claims: new[] {
        new Claim(ClaimTypes.Role,role),
        new Claim(ClaimTypes.NameIdentifier, userId)
});
    return new JwtSecurityTokenHandler().WriteToken(token);
}
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AddDb>();
    if (!db.users.Any(u => u.Login == "admin"))
    {
    db.users.Add(new Usered { Login = "admin", Password = "passAdmin", Role = "Admin" });
        db.SaveChanges();
    }
}


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AddDb>();
    if (!db.tickets.Any())
    {
    }
}
