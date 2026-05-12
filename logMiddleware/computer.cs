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
using SQLitePCL;
using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Avia.Middleware;
using Avia.Models;
using Avia.Interfaces;
using Microsoft.AspNetCore.Http;
using Avia.log;
using Avia.Main;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Azure.Core;
namespace Avia.Middleware.Computer;
public class Computer
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<string,string> _newtip = new()
    {
    { "Windows", "PC" },
    { "iPhone", "iOS" },
    { "Android", "Mobile" },
    { "Postman", "Postman" }
    };
    public Computer(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext http)
    {
        var agent = http.Request.Headers["User-Agent"].ToString();
        var platform = _newtip.FirstOrDefault(u => agent.Contains(u.Key)).Value ?? "Unknown";
        http.Response.Headers.Add("X-Detected-Platform", platform);
        await _next(http);
    }
}