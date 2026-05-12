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
using System.Collections;
using Microsoft.AspNetCore.Http.HttpResults;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata.Ecma335;
using Avia.Hz;
using Avia.Main;
using Avia.Models;
using System.Runtime;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlTypes;
namespace Avia.log;
public class BaseResponse<T>
{ 
    public bool Success {get;set;}
    public List<string> errors {get;set;} = new();
    public DateTime time {get;set;}
    public T data {get;set;}
    public string Message {get;set;}
    public bool seataviable {get;set;}
    public DateTime dateTime {get;set;}
    public static BaseResponse<T> Ok(T data,string message = "Success") => new() {data = data, Message = message, Success = true};
    public static BaseResponse<T> Fail(string message = "Error",List<string> Errors = null) => new() {Success = false, data = default , Message = message, errors = Errors};

    internal static IActionResult Ok()
    {
        throw new NotImplementedException();
    }
}
public static class Ife
{
    public static T ifs<T> (T? names) where T : class
    {
        if (names is null)
        {
            throw new Exception("object null");
        }
        if (names is IQueryable queryable && names is not null)
        {
            if (!queryable.Cast<object>().Any())
            {
                throw new Exception("nothing");
            }
        }
        else if(names is IEnumerable enumerable && names is not string)
        {
            if (!enumerable.Cast<object>().Any())
            {
                throw new Exception("nothing");
            }
        }
        return names;
        } 
    }

public static class UpdateEfcoreDbModel
    {
        public static T UpdateFrom<T> (T? entity, T? entity2,AddDb db) where T : class
        {
            if (entity is null || entity2 is null )
            {
                throw new Exception("nothing");
            }
            var type = typeof(T).GetProperties();

            foreach (var item in type)
            {
                if (item.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)) continue;
                if (item.CanRead && item.CanWrite)
                {
                    var newvalue = item.GetValue(entity2);
                    if (newvalue != null)
                    {
                        item.SetValue(entity, entity2);  
                    }
                }
            }
            return entity;
        }
    }
