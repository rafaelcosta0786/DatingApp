using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
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
      if (!IsAuthUser(userId))
        return Unauthorized();

      var messageFromRepo = await _repository.GetMessage(id);

      if (messageFromRepo == null)
        return NotFound();

      return Ok(messageFromRepo);
    }

    [HttpGet]
    public async Task<IActionResult> GetMessageForUser([FromRoute] int userId, [FromQuery] MessageRequest request)
    {
      if (!IsAuthUser(userId))
        return Unauthorized();

      request.UserId = userId;

      var messageFromRepo = await _repository.GetMessagesForUser(request);

      var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

      Response.AddPagination(messageFromRepo.CurrentPage, messageFromRepo.PageSize, messageFromRepo.TotalCount, messageFromRepo.TotalPages);

      return Ok(messages);

    }

    [HttpGet("thread/{recipientId}")]
    public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
    {
      if (!IsAuthUser(userId))
        return Unauthorized();

      var messageRepo = await _repository.GetMessageThread(userId, recipientId);

      foreach (var item in messageRepo.Where(x => x.IsRead == false && x.RecipientId == userId))
      {
        item.DateRead = DateTime.Now;
        item.IsRead = true;
        await _repository.SaveAll();
      }

      var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageRepo);
      return Ok(messageThread);
    }

    [HttpPost("{messageId}/delete")]
    public async Task<IActionResult> DeleteMessage(int userId, int messageId)
    {
      if (!IsAuthUser(userId))
        return Unauthorized();

      var messageFromRepo = await _repository.GetMessage(messageId);

      if (messageFromRepo.SenderId == userId)
        messageFromRepo.SenderDeleted = true;

      if (messageFromRepo.RecipientId == userId)
        messageFromRepo.RecipientDeleted = true;

      if (messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
        _repository.Delete(messageFromRepo);

      if (await _repository.SaveAll())
        return NoContent();

      return BadRequest("Failed to delete message");

    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto dto)
    {
      var sender = await _repository.GetUser(userId);
      if (!IsAuthUser(sender.Id))
        return Unauthorized();


      dto.SenderId = userId;

      var recipient = await _repository.GetUser(dto.RecipientId);

      if (recipient == null)
        return NotFound("Could not find user");

      var message = _mapper.Map<Message>(dto);

      _repository.Add(message);

      if (await _repository.SaveAll())
      {
        var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
        return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageToReturn);
      }

      throw new Exception("Creating the message failed on save");

    }

  }
}