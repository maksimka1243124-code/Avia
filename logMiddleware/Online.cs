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
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data.Common;
using FluentValidation.AspNetCore;
using SQLitePCL;
using System.Transactions;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Avia.Main;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.Connections;
using Avia.log;
namespace Avia.User.Online;
public class Online
{
    private readonly RequestDelegate _next;
    public Online(RequestDelegate next)
    {
        _next=  next;
    }
    public async Task Invoke(HttpContext http, AddDb db)
    {
        var usered = http.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(usered)) { await _next(http); return; }
        var userid = await db.users.FindAsync(int.Parse(usered));
        Ife.ifs(userid);
        var time = DateTime.UtcNow - userid.OnlineLater;
        if (time.TotalMinutes > 2 )
        {
            userid.OnlineNow = true;
            userid.OnlineLater = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        else
        {
            userid.OnlineNow = false;
        }
        await db.SaveChangesAsync();
        await _next(http);
    }
}