using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
  public class Seed
  {
    public static void SeedUsers(UserManager<Users> userManager, RoleManager<Role> roleManager)
    {
      if (!userManager.Users.Any())
      {
        var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
        var users = JsonConvert.DeserializeObject<List<Users>>(userData);

        var roles = new List<Role>(){
            new Role{Name ="Member"},
            new Role{Name ="Admin"},
            new Role{Name ="Moderator"},
            new Role{Name ="VIP"},
        };

        foreach (var role in roles)
        {
          roleManager.CreateAsync(role).Wait();
        }

        foreach (var user in users)
        {
          userManager.CreateAsync(user, "password").Wait();
          userManager.AddToRoleAsync(user, "Member").Wait();
        }


        var adminUser = new Users
        {
          UserName = "admin"
        };

        var resultAdmin = userManager.CreateAsync(adminUser, "password").Result;
        if (resultAdmin.Succeeded)
        {
          var admin = userManager.FindByNameAsync("admin").Result;
          userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
        }
      }
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {

      using (var hmac = new System.Security.Cryptography.HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      }
    }
  }
}