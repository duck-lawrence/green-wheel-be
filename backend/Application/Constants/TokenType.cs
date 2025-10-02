using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public enum TokenType
    {
        AccessToken = 0,
        RefreshToken = 1,
        RegisterToken = 2,
        ForgotPasswordToken = 3,
        SetPasswordToken = 4
    }
}
