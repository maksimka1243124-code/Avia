using System;
using System.Reflection.Metadata;
using System.Net.Sockets;
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
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using FluentValidation.Internal;
using System.Threading.Tasks;
namespace Avia.Main;
[ApiController]
[Route("api/[HomeController]")]
public class TravelController : BaseController
{
    private readonly IValidator<Travel> _validator;
    public TravelController(AddDb db,IValidator<Travel> validator) : base(db)
    {
        _validator = validator;
    }
    [HttpGet("tickets")]
    public async Task<IActionResult> Gettickets(int page,int pagesize,TicketDtoRequest ticketDtoRequest)
    {
        var page_ = HttpContext.Items["Pagination"] as Paginationed;
        var query = _db.tickets.AsQueryable();
        if (page_ != null)
        {
            query = query.Skip(page_.Skip).Take(page_.Take);
        }
        var resulted = await query.Select(x => new TicketDto
        {
            Name = x.Name,
            Price = x.price,
            dateTimeFly = x.dateTimeFly
        }).ToListAsync();
        return Ok(BaseResponse<List<TicketDto>>.Ok(resulted));
    }
    [Authorize]
    [HttpPut("tickets/buy/{id:guid}")]
    public async Task<BaseResponse<TicketDto>> BuyTicket(string id,string ticketid,string seatid, TicketDtoRequest ticketDtoRequest)
    {   
        var validators = new BaseResponse<TicketDto>();
        var resulted = await _validator.ValidateAsync((IValidationContext)ticketDtoRequest);
        if (!resulted.IsValid)
        {
            validators.Message = "error validation";
            validators.dateTime = DateTime.UtcNow;
            validators.Success = false;
            validators.errors = resulted.Errors.Select(u => u.ErrorMessage).ToList();
            return validators;
        }
        var useridsearch = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        var userid = int.Parse(useridsearch.Value);
        var seatsid = await _db.tickets.Include(u => u.seat).FirstOrDefaultAsync(u => u.SeatId == seatid);
        var ticket = await _db.tickets.Include(u => u.travel).Where(u => u.Id == id).FirstOrDefaultAsync();
        var seatsidtravel = await _db.tickets.FindAsync(seatsid);
        var traveled = ticket.travel;
        if (traveled.Status == "Rejected" && traveled.Status == "InProcess") throw new Exception("not active");
        if (ticket.seatAvailable == false) throw new Exception("место занято!");
        if (seatsid is null) throw new Exception("Место не найдено!");
        if (ticket is null) throw new Exception("Билет не найден");
        var dbuser = await _db.users.FindAsync(userid);
        Ife.ifs(dbuser);
        var tickets = await _db.tickets.Where(u => u.Id == id).Select(u => new TicketDto {
        id = u.Id,
        Price = u.price,
        IsAvilable = u.seatAvailable,
    }).FirstOrDefaultAsync();
    if (ticket.seatAvailable) throw new Exception("билет уже куплен!");
        if (dbuser.Money < ticket.price) throw new Exception("нет денег!");
            ticket.UserId = dbuser.Id;
            ticket.seatAvailable = true;
            ticket.seat.Id = dbuser.Id;
            ticket.seat.IsAvailable = false;
        await _db.SaveChangesAsync();
        return BaseResponse<TicketDto>.Ok(tickets);
        }
    [HttpGet("ticket/search")]
    public async Task<IActionResult> Search(decimal maxprice,decimal minprice)
    {
        var ticketmax = await _db.tickets.MaxAsync(u => u.price);
    if (minprice <= 0) throw new Exception("Цена должна быть больше нуля!");
    if (maxprice > ticketmax) throw new Exception("Максимальная цена не может быть больше чем цена билета!");
    var findid = _db.tickets.AsQueryable();
    Ife.ifs(findid);
        findid = findid.Where(u => u.price >= minprice && u.price <= maxprice);
        var _d = HttpContext.Items["Pagination"] as Paginationed;
        if (_d != null)
        {
            findid = findid.Skip(_d.Skip).Take(_d.Take);
        }
        var result = await findid.Select(c => new TicketDto
        {
            Price = c.price,
            Name = c.Name,
            dateTimeFly = c.dateTimeFly,
        }).ToListAsync();
        return Ok(BaseResponse<List<TicketDto>>.Ok(result));
    }
    [Authorize]
    [HttpGet("localcabinet/ticket")]
    public async Task<IActionResult> LocalcabinetTicket()
    {
        var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();
        var id = int.Parse(currentUserId);
        var user = await _db.users.FindAsync(id);
        if (user is null) return NotFound();
        var users = BaseResponse<UserDto>.Ok(new UserDto {Name = user.Name, dateTimeRegistration = user.dateTimeRegistration, Id = user.Id, Money = user.Money, IsBlocked = user.IsBlocked});
        return Ok(users);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("delete-flight")]
    public async Task<IActionResult> Deleteflight(string id)
    {
        var currentAdminId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (currentAdminId is null) throw new Exception("don't finded");
        if (!Guid.TryParse(id,out _))
        {
            throw new Exception("Don't guid!");
        }
            var delete = await _db.travels.Where(u => u.Id == id).Select(u => new TravelDto
        {
            Id = u.Id,
            Title = u.Title,
            IsdeletedAt = u.IsDeletedAt,
            MaxSeats = u.MaxSeats,
            Price = u.Price,
            }).FirstOrDefaultAsync();
        var dto = Ife.ifs(delete);
        var entity = new Travel {Id = id};
        _db.travels.Remove(entity);
        await _db.SaveChangesAsync(currentAdminId);
        return Ok(BaseResponse<TravelDto>.Ok(delete));
    }
    [Authorize]
    [HttpGet("flight")]
    public async Task<IActionResult> Flight(HttpContext http)
    {
        var h = http.Items["Pagination"] as Paginationed;
        var query = _db.travels.AsQueryable();
        if (h is not null)
        {
            query = query.Skip(h.Skip).Take(h.Take);
        }
        if (http.User.IsInRole("Admin"))
        {
            query = query.IgnoreQueryFilters();
        }
        if (!query.Any() && query is null ) throw new Exception("not query!");
        var dto = query.Select(u => new TravelDto
        {
            Title = u.Title,
            Price = u.Price, 
            Date = u.Date,
            IsdeletedAt = u.IsDeletedAt,
        }).ToList();
        return Ok(BaseResponse<List<TravelDto>>.Ok(dto));
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("localcabinet/ticket/change")]
    public async Task<IActionResult> TicketChange(string id,string ticketid,string newticketid)
    {
        var idUser = await _db.users.Include(u => u.tickets).FirstOrDefaultAsync(u => u.Id == id);
        Ife.ifs(idUser);
        var ticketone = idUser.tickets.FirstOrDefault(u => u.Id == ticketid);
        Ife.ifs(ticketone);
        var tickettwo = await _db.tickets.FirstOrDefaultAsync(u => u.Id == newticketid);
        Ife.ifs(tickettwo);
        UpdateEfcoreDbModel.UpdateFrom<Ticket>(ticketone,tickettwo, _db);
        _db.tickets.Remove(tickettwo);
        await _db.SaveChangesAsync();
        return Ok(BaseResponse<TicketDto>.Ok(new TicketDto {Name = ticketone.Name, dateTimeFly = ticketone.dateTimeFly, IsAvilable = true, Price = ticketone.price}));
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("Admin/check-list")]
    private async Task<IActionResult> GetCheckList()
    {
        var adminid = await _db.users.Where(u => u.Role == "admin").Select(u => new UserDto
        {
            Name = u.Name,
            OnlineNow = u.OnlineNow,
            Id = u.Id,
            IsBlocked = u.IsBlocked,
        }).ToListAsync();
        Ife.ifs(adminid);
        return Ok(BaseResponse<List<UserDto>>.Ok(adminid));
    }
    [Authorize(Roles = "GlAdmin")]
    [HttpDelete("admin/check-list")]
    public async Task<IActionResult> Deleted(string id)
    {
        var checkid = await _db.users.FindAsync(id);
        Ife.ifs(checkid);
        if (checkid.Role != "admin") throw new Exception("not admin");
        if (checkid.Role == "GlAdmin") throw new Exception("you don't can delete yourself");
        if (checkid.Isdelete == true) throw new Exception("admin has been deleted");
        _db.users.Remove(checkid);
        await _db.SaveChangesAsync();
        return Ok("good");
    }
    [Authorize(Roles = "Dispatcher")]
    [HttpGet("allflights")]
    public async Task<IActionResult> Allflight(BackgroundService backgroundService, HttpContext http, CancellationToken cancellationToken)
    {
        var pagination = http.Items["Pagination"] as Paginationed;
       Ife.ifs(pagination);
            var query = _db.travels.AsQueryable();
            if (!query.Any()) throw new Exception("NULL");
            query = query.Skip(pagination.Skip).Take(pagination.Take);
            var result = await query.Select(u => new TravelDto
            {
                Title = u.Title,
                Id = u.Id,
                Date = u.Date
            }).ToListAsync();
            return Ok(BaseResponse<List<TravelDto>>.Ok(result));
    }
    [Authorize(Roles = "Dispatcher" )]
    [HttpPost("allflights/change{id:guid}/{status}")]
    public async Task<IActionResult> Change(Guid id,string status)
    {
        var flightid = await _db.travels.FirstOrDefaultAsync(u => u.Id == id.ToString());
        Ife.ifs(flightid);
        if (status != "Approve" && status != "Rejected" && status != "InProcess") throw new Exception("not currently query");
        flightid.Status = status;
        await _db.SaveChangesAsync();
        return Ok(BaseResponse<TravelDto>.Ok(new TravelDto{Title = flightid.Title,Price = flightid.Price,Status = status}));
    }
    [Authorize]
    [HttpDelete("localcabinet/ticket/{guid:id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if(!string.IsNullOrEmpty(claim)) throw new Exception("nothing!");
        var userid = await _db.users.Include(u => u.tickets).FirstOrDefaultAsync(u => u.Id == claim);
        if (userid is null) throw new Exception("null user");
        var da = userid.tickets.FirstOrDefault(u => u.Id == id.ToString());
        if (da is null) throw new Exception("you don't have this ticket");
        da.UserId = null;
        da.seatAvailable = true;
        await _db.SaveChangesAsync();
        return Ok(BaseResponse<TicketDto>.Ok(new TicketDto {Name = da.Name, IsAvilable = da.seatAvailable, Price = da.price}));
    }
}   