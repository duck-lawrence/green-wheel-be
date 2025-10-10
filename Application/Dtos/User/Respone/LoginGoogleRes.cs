using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Respone
{
    public class LoginGoogleRes
    {
        public bool NeedSetPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AccessToken { get; set; }
    }
}
