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
   namespace Avia.Pa;
   public class TransMiddleware {
      private readonly RequestDelegate _next;
      public TransMiddleware(RequestDelegate transmidl)
      {
         _next = transmidl;
      }
   public async Task Invoke(HttpContext http, AddDb db)
   {
      if (http.Request.Method == "GET")
      {
         await _next(http);
         return;
      }
      using var trans = await db.Database.BeginTransactionAsync();
      try
      {
         await _next(http);
         await trans.CommitAsync();
      }
      catch (Exception ex)
      {
         await trans.RollbackAsync();
         throw;
      }
   }
   }