using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  public class DefaultController : ControllerBase
  {
    public bool IsAuthUser(int userId)
    {
      return (userId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
    }
  }
}