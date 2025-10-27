using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class UserChangePasswordReqValidator : AbstractValidator<UserChangePasswordReq>
    {
        public UserChangePasswordReqValidator()
        {

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Message.UserMessage.PasswordCanNotEmpty)
                .MinimumLength(6).WithMessage(Message.UserMessage.PasswordTooShort);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(Message.UserMessage.ConfirmPasswordIsIncorrect);
        }
    }
}
