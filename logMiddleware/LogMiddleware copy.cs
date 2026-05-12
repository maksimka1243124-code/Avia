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
using Avia.Main;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.Connections;
namespace Avia.Page;
public class Pagination
{
    private readonly RequestDelegate _next;
    public Pagination(RequestDelegate next)
    {
        _next = next;
    }
    HashSet<string> Apiadress = new() {"api/ticket/search"};
    public async Task Invoke(HttpContext http)
    {
        if (http.Request.Method == "GET")
        {
            var path = http.Request.Path.Value.ToLower();
            if (Apiadress.Contains(path))
            {
                int.TryParse(http.Request.Query["Page"], out var Page);
                int.TryParse(http.Request.Query["Pagesize"],out var Pagesize);
                var pagination = new Paginat(Page,Pagesize);
                http.Items["Pagination"] = pagination;
            }
        }
        await _next(http);
    }
}