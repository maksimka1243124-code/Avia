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
using Avia.Interfaces;
using Avia.IpAdress;
using Avia.Middleware;
using Avia.Middleware2;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Data.Common;
using FluentValidation.AspNetCore;
using SQLitePCL;
using System.Transactions;
using Avia.Main;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Avia.code;
namespace Avia.IpAdress;
public class IpLog 
{
    private readonly RequestDelegate _next;
    private readonly IidDelete _iid;
    private readonly Iticketservice _ticketService;
    public IpLog(RequestDelegate next,IidDelete iid, Iticketservice ticketService)
    {
        _next = next;
        _iid = iid;
        _ticketService = ticketService;
    }
    public async Task InvokeAsync(HttpContext http,IidDelete iid,Iticketservice ticketService)
    {
        var ip = http.Connection.RemoteIpAddress?.ToString();
        if (ip != null && _iid.IsBlock(ip))
        {
            http.Response.StatusCode = 403;
            return; 
        }
        await _next(http);
    }
}