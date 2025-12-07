using LibraryWebsite.Model;
using System.Collections.Generic;

namespace LibraryWebsite.Service
{
    public interface IUserService
    {
        bool Add(User user);
        User? GetById(int id);
        List<User> GetAll();
        bool Delete(int id);
        void Update(User user);
        bool? Login(string username, string password);

    }
}
