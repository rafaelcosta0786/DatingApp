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
    }
}