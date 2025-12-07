using LibraryWebsite.Model;
using LibraryWebsite.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryWebsite.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
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


        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public bool Add(User user)
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
            user.IsActive = true;
            user.Role = "user";
            user.CreatedAt = DateTime.Now;

            return _repo.Add(user);
        }



        public User GetById(int id) => _repo.GetById(id);

        public List<User> GetAll() =>
            _repo.GetAll().OrderByDescending(u => u.CreatedAt).ToList();

        public bool Delete(int id) => _repo.DeleteById(id);



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
                existingUser.PasswordHash = HashPassword(user.PasswordHash);

            existingUser.UpdatedAt = DateTime.Now;

            _repo.Update(existingUser);
        }



        public string Login(string username, string password)
        {
            var user = _repo.GetAll().FirstOrDefault(u => u.Username == username);
            if (user == null)
                return null;

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
                return null;

            return GenerateJwtToken(user);
        }

        bool? IUserService.Login(string username, string password)
        {
            return true;
        }
    }
}
