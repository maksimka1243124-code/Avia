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
using Avia.Interfaces;
using Avia.IpAdress;
using Avia.Middleware;
using Avia.Middleware2;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data.Common;
using FluentValidation.AspNetCore;
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
[ApiController]
[Route("api/[controller]")]
public class UserReg(AddDb db,IConfiguration config) : ControllerBase
{
    readonly AddDb db;
    readonly IValidator validator;
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] UserCreateDto userCreateDto,[FromServices] IValidator<UserCreateDto> validator)
    {
        var result = validator.Validate(userCreateDto);
        if (!result.IsValid) 
    {
        return BadRequest(result.Errors.Select(e => e.ErrorMessage));
    }
    var searchlog = await db.users.AnyAsync(u => u.Login == userCreateDto.login);
        if (searchlog) return BadRequest("пользователь зарегистрирован!");
        var passhash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.password);
        var usered = new Usered
        {
            Name = userCreateDto.name,
            Login = userCreateDto.login,
            Password = passhash,
            Role = "User",
            IsBlocked = false,
            dateTimeRegistration = DateTime.UtcNow,
        };
        db.users.Add(usered);
        await db.SaveChangesAsync();
        return Ok(new UserDto
        {
            Name = usered.Name,
            Login = usered.Login,
            IsBlocked = usered.IsBlocked,
            Role = usered.Role
        });
    }
    [Authorize]
    [HttpPost("login")]
    public async Task<IActionResult> Login(Usered usered,string password,string login,IConfiguration config)
    {        
        var log = await db.users.FirstOrDefaultAsync(u => u.Login == login);
        if (log is null) return BadRequest("Not the correct password or login!");
        var use = BCrypt.Net.BCrypt.Verify(password, usered.Password);
        if (!use) return Unauthorized();
        var token = CreateToken(usered);
        return Ok(new UserDto
        {
            Name = log.Name,
            dateTimeRegistration = log.dateTimeRegistration
        });
    }
        private string CreateToken(Usered user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
}
}