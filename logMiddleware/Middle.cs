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
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Avia.Main;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
namespace Avia.Middleware2;
public class Middleware
{
    private readonly RequestDelegate _next;
    public Middleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext http)
    {
        var method = http.Request.Method;
        var path = http.Request.Path;
        var timedata = DateTime.UtcNow;
        var ip = http.Connection.RemoteIpAddress?.ToString();
        await _next(http);
        var times = DateTime.UtcNow - timedata;
        var statuscode = http.Response.StatusCode;
        Console.ForegroundColor = statuscode switch
        {
            >= 500 => ConsoleColor.Red,
            >= 400 => ConsoleColor.Black,
            >= 300 => ConsoleColor.Blue,
            >= 200 => ConsoleColor.DarkYellow,
            _      => ConsoleColor.White,
        };
    Console.WriteLine($"{method} - {path} - {times} - {statuscode} - {DateTime.Now:HH:mm:ss} - {ip}");

    }
}