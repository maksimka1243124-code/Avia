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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.StaticAssets.Infrastructure;
using Avia.log;
namespace Avia.Data;
public class AddDb : DbContext
{
    public DbSet<Usered> users {get;set;}
    public DbSet<Travel> travels {get;set;}
    public DbSet<Ticket> tickets {get;set;} = null!;
    public DbSet<Seats> seats {get;set;}
    public DbSet<TicketDto> ticketDtos {get;set;}
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("data source =add.db");
    public (int,int) Getpage (int page,int pagesize)
   {
      if (page <= 0) page = 1;
      if (pagesize <= 0) pagesize = 10;
      return (page,pagesize);
   }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<Ticket>().HasOne(u => u.travel).WithMany(u => u.tickets).HasForeignKey(u => u.TravelId).OnDelete(DeleteBehavior.Cascade);
       modelBuilder.Entity<Ticket>().HasOne(u => u.User).WithMany(u => u.tickets).HasForeignKey(u => u.PassagerId);
       modelBuilder.Entity<Ticket>().HasQueryFilter(u => !u.seatAvailable);
       modelBuilder.Entity<Ticket>().HasIndex(u => u.price);
       modelBuilder.Entity<Seats>().HasOne(u => u.travel).WithMany(u => u.seats).HasForeignKey(u=> u.TravelId);
       modelBuilder.Entity<Travel>().HasQueryFilter(u => !u.IsDeleted);
    }
       public class JwtOptions
    {
        public const string name = "Jwt";
        [Required]
        [MinLength(32,ErrorMessage = "password too short"),MaxLength(500, ErrorMessage = "password too very big!")]
        public string key {get;set;} = string.Empty;
        public int ExpiresHours { get; set; } = 24;
    }
    public Task<int> SaveChangesAsync (string userId,CancellationToken cancellationToken = default)
   {
      var entres = ChangeTracker.Entries().Where(u => u.Entity is ISoftDeletable && u.State == EntityState.Added || u.State == EntityState.Modified || u.State == EntityState.Deleted);
         foreach (var da in entres)
         {
            var entr = (ISoftDeletable)da.Entity;
            var now = DateTime.UtcNow;
            switch (da.State)
         {
            case EntityState.Added:
            if (string.IsNullOrEmpty(entr.Id))
               {
                  entr.Id = Guid.NewGuid().ToString();
               }
            entr.CreatedAt = now;
            break;
            case EntityState.Deleted:
            da.State = EntityState.Modified;
            var entityss = (ISoftDeletable)da.Entity;
            entityss.IsDeletedAt = DateTime.UtcNow;
            entityss.IsDeleted = true;
            entityss.DeletedBy = userId;
            break;
         }
         }
      return base.SaveChangesAsync(cancellationToken);
   }
    }
