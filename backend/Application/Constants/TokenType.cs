using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum TokenType
    {
        AccessToken = 1,
        RefreshToken = 2,
        RegisterToken = 3,
        ForgotPasswordToken = 4,
        SetPasswordToken = 5
    }
}
