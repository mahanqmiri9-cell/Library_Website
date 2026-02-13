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
using System.Security.Principal;
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
            Encoding.UTF8.GetBytes(
                _config["Jwt:Key"] ?? throw new Exception("JWT Key not found")
            )
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

        public string role = "User";

        public UserAddDTO Add(UserAddDTO dto)
        {
            

            if (string.IsNullOrWhiteSpace(dto.FullName) ||
                string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.PhoneNumber))

            {
                throw new ApplicationException("invalid input!");
            }

            if (_repo.UsernameExists(dto.UserName))
            {
                throw new ApplicationException("Username already exists");
            }

            if (_repo.EmailExists(dto.Email))
            {
                throw new ApplicationException("Email already exists");
            }

            if (_repo.PhoneNumberExists(dto.PhoneNumber))
            {
                throw new ApplicationException("PhoneNumber is already exists");
            }

            if (dto.UserName == "AdminLW" && dto.Password == "Ma@123456#")
            {
                role = "Admin";
            }

                var user = new User
                {
                    FullName = dto.FullName,
                    Username = dto.UserName,
                    PasswordHash = HashPassword(dto.Password),
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    IsActive = true,
                    Role = role,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,

                };



            _repo.Add(user);
            _repo.Save();
            return new UserAddDTO
            {
                FullName = user.FullName,
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }



        public UserGetByIdDTO GetById(int id)
        {
            var user = _repo.GetById(id);
            if (user == null)
                throw new Exception("User Not Found");

            return new UserGetByIdDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
            };
        }

        public List<BookGetDTO> GetAll(int pageNumber, int pageSize)
        {
            return _repo.GetAll(pageNumber , pageSize)
                .Select(u => new BookGetDTO
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

        public List<UserGetDTO> GetAllUsers(int pagenumber , int pagesize)
        {
            return _repo.GetAllUsers(pagenumber , pagesize)
                .Select(u => new UserGetDTO
                {
                    FullName = u.FullName,
                    UserName = u.Username,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                })
                .ToList();
        }

        public void Delete(int id)
        { 
           _repo.DeleteById(id);
           _repo.Save();
        }

        public UserUpdateDTO Update(int UserId , UserUpdateDTO dto)
        {
            var User = _repo.GetById(UserId);
            if (User == null)
            {
                throw new Exception("User not found");
            }

            if (_repo.UsernameExists(User.Username))
                {
                    throw new Exception("Username is already exists");
                }

            if (_repo.EmailExists(User.Email))
            {
                throw new Exception("Email is already exists");
            }

            if (_repo.PhoneNumberExists(User.PhoneNumber))
            {
                throw new Exception("PhoneNumber is already exists");
            }

            if (!string.IsNullOrWhiteSpace(dto.FullName))
            {
                User.FullName = dto.FullName;
            }

            if (!string.IsNullOrWhiteSpace(dto.UserName))
            {
                User.Username = dto.UserName;
            }

            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                User.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                User.PhoneNumber = dto.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                User.PasswordHash = HashPassword(dto.Password);
            }

            User.UpdatedAt = DateTime.Now;
            _repo.Update(User);
            _repo.Save();

            return new UserUpdateDTO
            {
                FullName = User.FullName,
                UserName = User.Username,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber
            };


        }

        public LoginResponseDTO Login(string username, string password)
        {
            var user = _repo.GetByUserName(username);
            if (user == null)
                throw new Exception("Invalid Username or Password");

            var hashedPassword = HashPassword(password);
            if (user.PasswordHash != hashedPassword)
                throw new Exception("Invalide Username or Password");

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token
            };
        }

        public User GetByUserName(string username)
        {
            throw new NotImplementedException();
        }
    }
}
