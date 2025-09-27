using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class UserRegisterReq
    {
        public string  Password { get; set; }
        public string  ConfirmPassword { get; set; }
        public string FirstName  { get; set; }
        public string LastName  { get; set; }   
        public DateTimeOffset? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public int Sex { get; set; }

    }
}
