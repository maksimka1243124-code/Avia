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
using Avia.Middleware2;
using Avia.Models;
using Avia.Interfaces;
using Microsoft.AspNetCore.Http;
using Avia.log;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Avia.Interfaces;
    public interface ISoftDeletable
{
    string Id {get;set;}
    bool IsDeleted { get; set; }
    string? DeletedBy {get;set;}
     DateTime CreatedAt { get; set; }
      DateTime? UpdatedAt { get; set; }
      DateTime IsDeletedAt {get;set;}
}
public interface IidDelete
{
    bool IsBlock(string ip);
}
public interface Pagination
{
    Task<BaseResponse<TicketDto>> Pagination (int page,int pagesize,AddDb db);
}
public interface Paginationed
{
    int Page {get;}
    int Pagesize {get;}
    int Skip => (Page-1) * Pagesize;
    int Take => Pagesize;
}
public class Paginat : Paginationed
{
    public int Page {get;}
    public int Pagesize {get;}
    public Paginat(int page, int pagesize)
    {
        Page = page <= 0  ? 1 : page;
        Pagesize = pagesize <= 0 ? 10 : pagesize;
    }
}
public interface ITicketBuilder
{
    Task<BaseResponse<TicketDto>> buyasync (string seatid,string userid);
}