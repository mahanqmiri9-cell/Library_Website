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
        public List<User> GetAll(int PageNumber , int PageSize)
        {
             return _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip((PageNumber-1)*PageSize)
                .Take(PageSize)
                .Select(u => new User 
                { 
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                })
                .ToList();
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id) ;
        }

        public bool Update(User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }




        public bool DeleteById(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);
            if (user == null)
            {
                return false;
            }
                                           //// ina jash to service
            _context.Users.Remove(user);
            return _context.SaveChanges() > 0;
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
