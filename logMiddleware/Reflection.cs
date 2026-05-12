using System;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
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
using Avia.Main;
using System.Reflection.Metadata.Ecma335;
using Avia.log;
using Microsoft.AspNetCore.Routing.Tree;
namespace Avia.Middleware.Reflection;
public class Reflection
{
    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string,Iact> _cache = new();
    public Reflection(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext http,IidDelete iid)
    {
        var ips = http.Connection.RemoteIpAddress?.ToString();
        Ife.ifs(ips);
        if (_cache.TryGetValue(ips,out var info))
        {
            var times = DateTime.UtcNow - info.lastreq;
            if (times.TotalMinutes < 1)
            {
                info.requestcount++;
                if (info.requestcount > 10)
                {
                    iid.IsBlock(ips);
                    throw new Exception("relax bro!");
                }
                else
                {
                    info.lastreq = DateTime.UtcNow;
                    info.requestcount++;
                }
            }
            else 
            {
                _cache.TryAdd(ips, new Iact { lastreq = DateTime.UtcNow, requestcount = 1 });
            }
            await _next(http);
        }
    }
}
    public class Iact
    {
        public DateTime lastreq {get;set;}
        public int requestcount {get;set;}
    }
