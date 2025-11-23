using System;
using LibraryWebsite.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebsite.Repository
{

        public interface IUserRepository
        { 
            bool Add(User user);
            DbSet<User> GetAll();
            User GetById (int id);
            void Update(User user);
            bool DeleteById(int id);
        }

}
