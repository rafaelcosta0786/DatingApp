using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
  [Authorize]
  [Route("api/users/{userId}/[controller]")]
  [ApiController]
  public class MessageController : DefaultController
  {
    private readonly IDatingRepository _repository;
    private readonly IMapper _mapper;
    public MessageController(IDatingRepository repository, IMapper mapper)
    {
      _mapper = mapper;
      _repository = repository;

    }

    [HttpGet("{id}", Name = "GetMessage")]
    public async Task<IActionResult> GetMessage(int userId, int id)
    {
      if (IsAuthUser(userId))
        return Unauthorized();

      var messageFromRepo = await _repository.GetMessage(id);

      if (messageFromRepo == null)
        return NotFound();

      return Ok(messageFromRepo);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto dto)
    {
      if (IsAuthUser(userId))
        return Unauthorized();

      dto.SenderId = userId;

      var recipient = await _repository.GetUser(dto.RecipientId);

      if (recipient == null)
        return NotFound("Could not find user");

      var message = _mapper.Map<Message>(dto);

      _repository.Add(message);

      if (await _repository.SaveAll())
      {
        var messageToReturn = _mapper.Map<MessageForCreationDto>(message);
        return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageToReturn);
      }

      throw new Exception("Creating the message failed on save");

    }

  }
}