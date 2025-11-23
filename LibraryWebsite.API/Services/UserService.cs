using LibraryWebsite.Model;
using LibraryWebsite.Repository;
using System.Collections.Generic;
using System.Linq;

namespace LibraryWebsite.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public bool Create(User user)
        {
            return _repo.Add(user);
        }

        public User Get(int id)
        {
            return _repo.GetById(id);
        }

        public List<User> GetAll()
        {
            return _repo.GetAll().ToList();
        }

        public bool Delete(int id)
        {
            return _repo.DeleteById(id);
        }

        public void Update(User user)
        {
            _repo.Update(user);
        }
    }
}
