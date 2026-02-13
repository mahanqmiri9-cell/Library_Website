using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryWebsite.Service.DTOs
{
    public class UserUpdateDTO
    {
        public string ?FullName { get; set; }
        public string ?UserName { get; set; }
        public string ?Email { get; set; }
        public string ?Password { get; set; }
        public string ?PhoneNumber { get; set; }
    }
}
