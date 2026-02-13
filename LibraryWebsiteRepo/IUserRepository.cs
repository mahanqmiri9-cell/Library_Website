using System;
using LibraryWebsite.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Azure;

namespace LibraryWebsite.Repository
{

        public interface IUserRepository
        { 
            void Add(User user);
            List<Book> GetAll(int PageNumber , int PageSize);
            List<User> GetAllUsers(int pagenumber, int pagesize);
            User ?GetById (int id);
            User ?GetByUserName (string userName);
            void Update(User user);
            void DeleteById(int id);
            bool UsernameExists(string username);
            bool EmailExists(string email);
            bool PhoneNumberExists(string phoneNumber);
            int Save();
        }

}
