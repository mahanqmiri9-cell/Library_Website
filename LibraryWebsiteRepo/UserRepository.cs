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

        public void Add(User user)
        {
            _context.Users.Add(user);
        }
        public List<Book> GetAll(int PageNumber , int PageSize)
        {
             return _context.Books
                .OrderByDescending(u => u.CreatedAt)
                .Skip((PageNumber-1)*PageSize)
                .Take(PageSize)
                .Select(u => new Book 
                { 
                    Title = u.Title,
                    ISBN = u.ISBN,
                    Categoryid = u.Categoryid,
                    Aythorid = u.Aythorid,
                    Dercription = u.Dercription,
                    PublishYear = u.PublishYear,
                    TotalCopies = u.TotalCopies,
                    AvaillableCopies = u.AvaillableCopies
                })
                .ToList();
        }

        public List<User> GetAllUsers(int pagenumber , int pagesize)
        {
            return _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Skip((pagenumber - 1) * pagesize)
                .Take(pagesize)
                .Select(u => new User
                {
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt

                })
                .ToList();
        }
        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id) ;
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void DeleteById(int id)
        {
            var user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }
            _context.Users.Remove(user);
        }

        public bool UsernameExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool PhoneNumberExists(string phoneNumber)
        {
            return (_context.Users.Any(u => u.PhoneNumber == phoneNumber));
        }

        public User? GetByUserName(string username)
        {
            if(username == null)
            {
                throw new Exception("User not Found");
            }

            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
