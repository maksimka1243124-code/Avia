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
using Avia.Main;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Avia.code;
public class Iticketservice : ITicketBuilder
{
    private readonly AddDb _db;
    private readonly Iticketservice _ticket;
    public Iticketservice(string seatid, string userid,Iticketservice ticket)
    {
        _ticket = ticket;
    }
    public async Task<BaseResponse<TicketDto>> buyasync (string seatid,string userid)
    {
        var searchseat = await _db.seats.Include(u => u.travel).FirstOrDefaultAsync(u => u.Id == seatid && u.IsAvailable == true);
        if (searchseat is null) return new BaseResponse<TicketDto> {Message = "seat not found"};
        var useridsearch = await _db.users.FirstOrDefaultAsync(u=> u.Id == userid);
        if (useridsearch is null) return new BaseResponse<TicketDto> {Message = "id not found"};
            var total = useridsearch.Money;
            var priceseat = searchseat.travel.Price;
            if (useridsearch.Money < searchseat.travel.Price)
            return new BaseResponse<TicketDto> { Message = "not enough money" };
            useridsearch.Money -= searchseat.travel.Price;
            searchseat.IsAvailable = false;
            var ticket = new Ticket
            {
                UserId = useridsearch.Id,
                SeatId = searchseat.Id.ToString(),
            };
            _db.tickets.Add(ticket);
            var ticketDto = new TicketDto
            {
                Price = searchseat.travel.Price,
                SeatNumber = searchseat.Id.ToString(),
            };
            await _db.SaveChangesAsync();
            return new BaseResponse<TicketDto>
            {
                data = ticketDto,
                Message = "already purchased"
            };
        }
}