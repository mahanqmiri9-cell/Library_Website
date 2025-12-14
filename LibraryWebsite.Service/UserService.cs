using LibraryWebsite.Model;
using LibraryWebsite.Repository;
using LibraryWebsite.Service.DTOs;
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

            if (_repo.UsernameExists(user.Username))
            {
                throw new Exception("Username already exists");
            }

            if (_repo.EmailExists(user.Email))
            {
                throw new Exception("Email already exists");
            }

            user.PasswordHash = HashPassword(user.PasswordHash);
            user.IsActive = true;
            user.Role = "user";
            user.CreatedAt = DateTime.Now;

            return _repo.Add(user);
        }



        public UserGetByIdDTO? GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
                return null;

            return new UserGetByIdDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
        }

        public List<UserGetDTO> GetAll(int pageNumber, int pageSize)
        {
            return _repo.GetAll()
                .OrderByDescending(u => u.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserGetDTO
                {
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt
                })
                .ToList();
        }

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


            var updated = _repo.Update(existingUser);
            if (!updated)
                throw new Exception("Update failed");

        }



        public LoginResponseDTO Login(string username, string password)
        {
            var user = _repo.GetAll().FirstOrDefault(u => u.Username == username);
            if (user == null)
                return null;

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
                return null;

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token
            };
        }



    }
}
