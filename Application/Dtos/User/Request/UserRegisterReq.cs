using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class UserRegisterReq
    {
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Sex { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }
}