using LibraryWebsite.Model;
using System.Collections.Generic;

namespace LibraryWebsite.Api.Services
{
    public interface IUserService
    {
        bool Create(User user);
        User Get(int id);
        List<User> GetAll();
        bool Delete(int id);
        void Update(User user);
        void Update(User user, int id);
    }
}
