using System.ComponentModel.DataAnnotations;

namespace LibraryWebsite.Service.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
