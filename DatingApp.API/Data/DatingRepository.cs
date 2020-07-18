using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Request;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
  public class DatingRepository : IDatingRepository
  {
    private readonly DataContext _context;
    public DatingRepository(DataContext context)
    {
      _context = context;

    }

    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }

    public async Task<LikeUser> GetLike(int userId, int recipientId)
    {
      return await _context.LikeUser.FirstOrDefaultAsync(u => u.LikeOriginUserId == userId && u.LikeDestinyUserId == recipientId);
    }

    public async Task<Message> GetMessage(int id)
    {
      return await _context.Message.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PagedList<Message>> GetMessagesForUser(MessageRequest request)
    {
      var messages = _context.Message
        .Include(u => u.Sender).ThenInclude(i => i.Photos)
        .Include(u => u.Recipient).ThenInclude(i => i.Photos)
        .AsQueryable();

      switch (request.MessageContainer)
      {
        case "Inbox":
          messages = messages.Where(x => x.RecipientId == request.UserId);
          break;
        case "Outbox":
          messages = messages.Where(x => x.SenderId == request.UserId);
          break;
        default:
          messages = messages.Where(x => x.RecipientId == request.UserId && x.IsRead == false);
          break;
      }

      messages = messages.OrderByDescending(x => x.MessageSent);
      return await PagedList<Message>.CreateAsync(messages, request.PageNumber, request.PageSize);
    }

    public Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
    {
      throw new NotImplementedException();
    }

    public async Task<Photo> GetPhoto(int id)
    {
      return await _context.Photo.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Users> GetUser(int id)
    {
      var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == id);
      return user;
    }

    public async Task<PagedList<Users>> GetUsers(UserForListRequest param)
    {
      var users = _context.Users.Include(p => p.Photos).AsQueryable();
      users = users.Where(x => x.Id != param.UserId);
      users = users.Where(x => x.Gender == param.Gender);

      if (param.LikeUserOrigin)
      {
        var userOrigins = await GetUserLikes(param.UserId, param.LikeUserOrigin);
        users = users.Where(x => userOrigins.Contains(x.Id));

      }
      if (param.LikeUserDestiny)
      {
        var userDestinys = await GetUserLikes(param.UserId, param.LikeUserOrigin);
        users = users.Where(x => userDestinys.Contains(x.Id));
      }


      if (param.MinAge != 18 || param.MaxAge != 99)
      {
        var minBirth = DateTime.Today.AddYears(-param.MaxAge - 1);
        var maxBirth = DateTime.Today.AddYears(-param.MinAge);

        users = users.Where(x => x.DateOfBirth >= minBirth && x.DateOfBirth <= maxBirth);
      }

      if (!string.IsNullOrEmpty(param.OrderBy))
      {
        switch (param.OrderBy)
        {
          case "created":
            users = users.OrderByDescending(u => u.Created);
            break;
          default:
            users = users.OrderByDescending(u => u.LastActived);
            break;
        }
      }

      return await PagedList<Users>.CreateAsync(users, param.PageNumber, param.PageSize);
    }

    public async Task<bool> SaveAll()
    {
      return await _context.SaveChangesAsync() > 0;
    }


    private async Task<IEnumerable<int>> GetUserLikes(int id, bool userOrigin)
    {
      var user = await _context.Users
                                .Include(x => x.LikeOriginUsers)
                                .Include(x => x.LikeDestinyUsers)
                                .FirstOrDefaultAsync(x => x.Id == id);

      if (userOrigin)
      {
        return user.LikeOriginUsers.Where(x => x.LikeDestinyUserId == id).Select(i => i.LikeOriginUserId);
      }
      else
      {
        return user.LikeDestinyUsers.Where(x => x.LikeOriginUserId == id).Select(i => i.LikeDestinyUserId);
      }
    }
  }
}