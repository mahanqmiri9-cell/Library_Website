using LibraryWebsite.Service;
using LibraryWebsite.Service.DTOs;
using LibraryWebsite.Model;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost]
        public IActionResult Creat(User user)
        {
            bool result = _service.Add(user);
            if (result)
                return Ok("User created successfully");

            return BadRequest("Failed");
        }



        [HttpGet]
        public IActionResult GetAll(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var users = _service.GetAll(pageNumber, pageSize);
            return Ok(users);
        }



        [HttpPut]
        public IActionResult Update(User user)
        {
            _service.Update(user);
            return Ok("User updated");
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }



        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
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
