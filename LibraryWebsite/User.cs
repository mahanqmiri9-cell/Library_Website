using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebsite.Model
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FullName is required")]

        public string FullName { get; set; }

        [Required(ErrorMessage = "Username is required")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]

        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]

        public string Email { get; set; }


        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = false;


        public string Role { get; set; } = "user";

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
