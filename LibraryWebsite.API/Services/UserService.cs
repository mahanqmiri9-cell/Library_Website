using LibraryWebsite.Model;
using LibraryWebsite.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibraryWebsite.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }


        public bool Create(User user)
        {
            
            if (string.IsNullOrWhiteSpace(user.FullName) ||
                string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                return false;
            }

            
            var existingUsers = _repo.GetAll();
            if (existingUsers.Any(u => u.Username == user.Username))
                return false; 

            if (existingUsers.Any(u => u.Email == user.Email))
                return false; 

            
            user.PasswordHash = HashPassword(user.PasswordHash);

            
            user.IsActive = false;
            user.Role = "user";
            user.CreatedAt = DateTime.Now;

            
            return _repo.Add(user);
        }


        public User Get(int id)
        {
            return _repo.GetById(id);
        }

        public List<User> GetAll()
        {
            return _repo.GetAll()
                        .OrderByDescending(u => u.CreatedAt)
                        .ToList();
        }

        public bool Delete(int id)
        {
            return _repo.DeleteById(id);
        }


        public void Update(User user)
        {
            
            var existingUser = _repo.GetById(user.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            
            var allUsers = _repo.GetAll();
            if (allUsers.Any(u => u.Id != user.Id && u.Username == user.Username))
                throw new Exception("Username already exists");

            if (allUsers.Any(u => u.Id != user.Id && u.Email == user.Email))
                throw new Exception("Email already exists");

            
            existingUser.FullName = user.FullName;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsActive = user.IsActive;
            existingUser.Role = user.Role;

            
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                existingUser.PasswordHash = HashPassword(user.PasswordHash);
            }

            existingUser.UpdatedAt = DateTime.Now;

            
            _repo.Update(existingUser);
        }

        public bool Login(string username, string password)
        {
            
            var user = _repo.GetAll().FirstOrDefault(u => u.Username == username);
            if (user == null)
                return false; 

            
            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
                return false; 

            return true; 
        }
    }


}
