using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Request;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<PagedList<Users>> GetUsers(UserForListRequest param);
         Task<Users> GetUser(int id);

         Task<Photo> GetPhoto(int id);
    }
}