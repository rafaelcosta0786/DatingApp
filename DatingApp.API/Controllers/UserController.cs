using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IDatingRepository _repo;
    private readonly IMapper _mapper;

    public UsersController(IDatingRepository repo, IMapper mapper)
    {
      _mapper = mapper;
      _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserForListRequest param)
    {
      var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
      var gender = User.FindFirst(ClaimTypes.Gender).Value;
      param.UserId = currentUserId;

      if (string.IsNullOrEmpty(param.Gender))
      {
        param.Gender = gender == "male" ? "female" : "male";
      }

      var users = await _repo.GetUsers(param);
      var userToDto = _mapper.Map<IEnumerable<UserForListDto>>(users);

      Response.AddPagination(users.CurrentPage,
                            users.PageSize,
                            users.TotalCount,
                            users.TotalPages);
      return Ok(userToDto);
    }

    [HttpGet("{id}", Name = "GetUser")]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
      var user = await _repo.GetUser(id);
      var userToDto = _mapper.Map<UserForDetailDto>(user);
      return Ok(userToDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserForUpdateDto userDto)
    {
      if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var userFromRepo = await _repo.GetUser(id);
      var userToDto = _mapper.Map(userDto, userFromRepo);
      if (await _repo.SaveAll())
        return NoContent();

      throw new System.Exception($"Updating user {id} failed on save");

    }
  }
}