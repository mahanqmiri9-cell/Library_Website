using LibraryWebsite.Api.Services;
using LibraryWebsite.Model;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebsite.Api.Controllers
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
        public IActionResult Create(User user)
        {
            bool result = _service.Create(user);
            if (result)
                return Ok("User created successfully");

            return BadRequest("Failed");
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _service.Get(id);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _service.GetAll();
            return Ok(users);
        }


        [HttpPut]
        public IActionResult Update(User user)
        {
            _service.Update(user);
            return Ok("User updated");
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _service.Delete(id);

            if (result)
                return Ok("User deleted");

            return BadRequest("failed");
        }


        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and Password are required");

            bool result = _service.Login(request.Username, request.Password);
            if (result)
                return Ok("Login successful");

            return Unauthorized("Invalid username or password");
        }



    }
}
