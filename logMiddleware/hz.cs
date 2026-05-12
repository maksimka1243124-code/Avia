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
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using avia.check.tickets;
namespace Avia.Hz;
public class Checker
{
    private readonly RequestDelegate _next;
    public Checker(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext http)
    {
       try
        {
            await _next(http);
        }
        catch (Exception ex) 
        {
        await HandleExpertAsync(http,ex);
        }
    }
    private static Task HandleExpertAsync(HttpContext http,Exception ex)
    {
        http.Response.ContentType = "application/json";
        http.Response.StatusCode = 500;
        var reponse = BaseResponse<object>.Fail(ex.Message);
        return http.Response.WriteAsJsonAsync(reponse);
    }
}