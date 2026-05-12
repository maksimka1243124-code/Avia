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
using Avia.Interfaces;
using Avia.IpAdress;
using Avia.Middleware;
using Avia.Middleware2;
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
using Avia.Models;
using Azure.Core.Pipeline;
using System.Xml.Linq;
using Avia.Data;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.Routing;
using FluentValidation.Validators;
using Avia.log;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using Avia.basecontroller;
namespace Avia.basecontroller;
[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly AddDb _db;
    public BaseController(AddDb db)
    {
        _db = db;
    }
}