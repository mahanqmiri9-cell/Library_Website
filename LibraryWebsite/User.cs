using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebsite.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = false;
        public string Role { get; set; } = "user";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
    }
}
