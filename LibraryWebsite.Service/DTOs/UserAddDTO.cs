using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebsite.Service.DTOs
{
    public class UserAddDTO
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string PhoneNumber { get; set; } = null!;
    }
}
