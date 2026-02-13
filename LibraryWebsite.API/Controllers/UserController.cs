using LibraryWebsite.Model;
using LibraryWebsite.Service;
using LibraryWebsite.Service.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LibraryWebsite.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [Authorize]
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Creat(UserAddDTO user)
        {
            try
            {
                _service.Add(user);
                return Ok("User creat successfully");
            }

            catch(ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
        [Authorize("all-users")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllUsers(
            int pagenumber = 1,
            int pagesize = 10)
        {
            var Users = _service.GetAllUsers(pagenumber, pagesize);
            return Ok(Users);
        }

        [HttpGet("books")]
        public IActionResult GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var Books = _service.GetAll(pageNumber, pageSize);
            return Ok(Books);
        }


        [Authorize]
        [HttpPut("me")]
        public IActionResult Update(UserUpdateDTO dto)
        {
            var UserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (UserIdClaim == null)
            {
                return Unauthorized();
            }

            int UserId = int.Parse(UserIdClaim.Value);

            try
            {
                _service.Update(UserId, dto);
                return Ok("Your profile Updated successfully");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [Authorize(Roles ="Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }





        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Username and Password are required");

            var response = _service.Login(dto.Username, dto.Password);
            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);

        }




    }
}
