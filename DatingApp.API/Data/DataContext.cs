using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
  public class DataContext : IdentityDbContext<Users, Role, int,
  IdentityUserClaim<int>, UsersRole, IdentityUserLogin<int>,
  IdentityRoleClaim<int>, IdentityUserToken<int>>
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Carro> Carro { get; set; }
    public DbSet<Photo> Photo { get; set; }

    public DbSet<LikeUser> LikeUser { get; set; }
    public DbSet<Message> Message { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<UsersRole>(userRole =>
      {
        userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

        userRole.HasOne(ur => ur.Role)
          .WithMany(r => r.UsersRoles)
          .HasForeignKey(ur => ur.RoleId)
          .IsRequired();

        userRole.HasOne(ur => ur.User)
          .WithMany(r => r.UsersRoles)
          .HasForeignKey(ur => ur.UserId)
          .IsRequired();          
        
      });

      builder.Entity<LikeUser>()
          .HasKey(k => new { k.LikeOriginUserId, k.LikeDestinyUserId });

      builder.Entity<LikeUser>()
        .HasOne(u => u.LikeDestinyUser)
        .WithMany(u => u.LikeOriginUsers)
        .HasForeignKey(u => u.LikeDestinyUserId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<LikeUser>()
        .HasOne(u => u.LikeOriginUser)
        .WithMany(u => u.LikeDestinyUsers)
        .HasForeignKey(u => u.LikeOriginUserId)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Message>()
        .HasOne(u => u.Sender)
        .WithMany(u => u.MessageSent)
        .OnDelete(DeleteBehavior.Restrict);

      builder.Entity<Message>()
        .HasOne(u => u.Recipient)
        .WithMany(u => u.MessageReceived)
        .OnDelete(DeleteBehavior.Restrict);

    }
  }
}