using System.Collections.Generic;
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
using Avia.IpAdress;
using Avia.Middleware;
using Avia.Middleware2;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Avia.Data;
using Microsoft.Net.Http.Headers;
namespace Avia.Models;
public class Usered
{
    public bool OnlineNow {get;set;}
    public DateTime OnlineLater {get;set;}
    public string Id {get;set;}
    public string Login {get;set;}
    public DateTime dateTimeRegistration {get;set;}
    public string Password {get;set;}
    public string Name {get;set;}
    public decimal Money {get;set;}
    public string Role {get;set;}
    public bool Isdelete {get;set;}
    public bool IsBlocked {get;set;}
    public string Token {get;set;}
    public List<Ticket> tickets {get;set;}
}
public class Ip : IidDelete
{
    public HashSet<string> IpAdress = new() {"127.0.0.0.1"};
    public bool IsBlock(string ip)
    {
        var dep = IpAdress.Contains(ip);
        return dep;
    }
}
public class Seats
{
    public string Id {get;set;}
    public string? UserId {get;set;}
    public bool IsAvailable {get;set;} = true;
    public string TravelId {get;set;}
    public Travel travel {get;set;}

}
public class Travel : ISoftDeletable
{
    public string Id {get;set;}
    public string Title {get;set;}
    public string Status {get;set;}
    public DateTime IsDeletedAt {get;set;}
    public int Date {get;set;}
    public string? DeletedBy {get;set;}
    public DateTime CreateTime {get;set;}
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime Time {get;set;}
    public List<Seats> seats {get;set;}
    public bool Current {get;set;}
    public decimal Price {get;set;}
    public bool IsDeleted {get;set;} = false;
    public int MaxSeats { get; set;}
    public Ticket Ticket {get;set;}
    public List<Ticket> tickets {get;set;}
}
public class TravelDto
{
    public string Status {get;set;}
    public string Id {get;set;}
    public string Title {get;set;}
    public DateTime IsdeletedAt {get;set;}
    public int Date {get;set;}
    public decimal Price {get;set;}
    public int MaxSeats {get;set;}
    public List<Seats> seats {get;set;}
    public Ticket ticket {get;set;}
}
public class UserDto 
{
    public bool OnlineRightNow {get;set;}
    public bool OnlineNow {get;set;}
    public string Id {get;set;}
    public string Role {get;set;}
    public DateTime dateTimeRegistration {get;set;}
    public string Login {get;set;}
    public string Name {get;set;}
    public decimal Money {get;set;}
    public bool Isdelete {get;set;}
    public bool IsBlocked {get;set;}
}
public class UserCreateDto
{
    public string login {get;set;}
    public string password {get;set;}
    public string name {get;set;}
    public class RegistrationValidator : AbstractValidator<UserCreateDto>
{
    public RegistrationValidator()
    {
        RuleFor(x => x.login).NotEmpty().MinimumLength(5);
        RuleFor(x => x.password).NotEmpty().MinimumLength(10).WithMessage("Пароль слишком короткий!");
    }
}
}
public class JwtOptions
{
    public string key { get; set; }
    public int ExpiresHours { get; set; }
}
public class Ticket
{
    public string Id{get;set;}
    public string TravelId {get;set;}
    public string UserId {get;set;}
    public decimal price {get;set;}
    public string Passanger {get;set;}
    public Usered User {get;set;}
    public Travel travel {get;set;}
    public DateTime dateTimeFly {get;set;}
    public string PassagerId {get;set;}
    public Seats seat {get;set;}
    public bool seatAvailable {get;set;} = true;
    public string SeatId {get;set;} 
    public string SeatNumber {get;set;}
    public string Name;
}
public class TicketDto
{
    public string id;
    public string Name;
    public string PassagerName {get;set;}
    public DateTime dateTimeFly {get;set;}
    public decimal Price {get;set;}
    public string SeatNumber { get; set;}
    public bool IsAvilable {get;set;}
    public int PassagerId {get;set;}
}
public class TicketDtoRequest() : TicketDto // ПЕРЕПИСАТЬ САМОМУ НАУЧИТСЯ!!! 
{
        public string SeatNumber {get;set;}
}
    public class ValidatorTicketSeat : AbstractValidator<TicketDto>
       {
      public ValidatorTicketSeat()
      {
         RuleFor(u => u.PassagerName).
         NotEmpty().WithMessage("Имя не может быть пустым").
         MinimumLength(6).WithMessage("Имя не может быть короче 6").
         MaximumLength(20).WithMessage("Имя не может быть таким длинным!");
      }
      }
      public class ValidatorEd : AbstractValidator<TicketDtoRequest> // ПЕРЕПИСАТЬ САМОМУ НАУЧИТСЯ!!! 
      {
         public ValidatorEd() // ПЕРЕПИСАТЬ САМОМУ НАУЧИТСЯ!!! 
         {
            Include (new ValidatorEd());
            RuleFor(x => x.SeatNumber).NotEmpty().WithMessage("Не может быть пустым!");
         }
    }
