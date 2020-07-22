using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AdminController : ControllerBase
  {
    private readonly DataContext _context;
    private readonly UserManager<Users> _userManager;
    public AdminController(DataContext context, UserManager<Users> userManager)
    {
      _userManager = userManager;
      _context = context;
    }

    [HttpGet("users-with-roles")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
      var userList = await _context.Users
                            .OrderBy(x => x.UserName)
                            .Select(user =>
                            new
                            {
                              Id = user.Id,
                              UserName = user.UserName,
                              Roles = (from userRole in user.UsersRoles
                                       join role in _context.Roles
                                           on userRole.RoleId equals role.Id
                                       select role.Name).ToList()
                            }).ToListAsync();

      return Ok(userList);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{userName}")]
    public async Task<IActionResult> EditRoles(string userName, RoleEditDto dto)
    {
      var user = await _userManager.FindByNameAsync(userName);

      var userRoles = await _userManager.GetRolesAsync(user);

      var selectedRoles = dto.RoleNames;

      selectedRoles = selectedRoles ?? new string[] { };
      var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

      if (!result.Succeeded)
        return BadRequest("Failed to add to roles");

      result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

      if (!result.Succeeded)
        return BadRequest("Failed to remove to roles");

      return Ok(await _userManager.GetRolesAsync(user));
    }

    [HttpGet("photos-moderation")]
    [Authorize(Policy = "RequirePhotoRole")]
    public IActionResult GetPhotosForModeration()
    {
      return Ok("Admin or Moderators can see this");
    }
  }
}