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
            return true;
        }
        public DbSet<User> GetAll()
        {
            return null;
        }
        public User GetById(int d)
        {
            return null;
        }
        public void Update(User user)
        { 
        }
        public bool DeleteById(int id)
        {
            return false;
        }
    }
}
