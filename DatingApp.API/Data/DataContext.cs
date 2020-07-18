using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Carro> Carro { get; set; }
    public DbSet<Users> Users { get; set; }
    public DbSet<Photo> Photo { get; set; }

    public DbSet<LikeUser> LikeUser { get; set; }
    public DbSet<Message> Message { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
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