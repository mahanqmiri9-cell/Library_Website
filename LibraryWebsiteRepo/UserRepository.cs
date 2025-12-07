using System;
using LibraryWebsite.Model;
using LibraryWebsite.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebsite.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(User user)
        {
            _context.Users.Add(user);
            return _context.SaveChanges() > 0;
        }
        public List<User> GetAll()
        {
             return _context.Users.ToList();
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id) ;
        }
        public void Update(User user)
        { 
            _context.SaveChanges(); //*incorrect*
        }
        public bool DeleteById(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }
    }
}
