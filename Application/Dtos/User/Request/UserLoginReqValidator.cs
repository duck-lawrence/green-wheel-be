using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class UserLoginReqValidator : AbstractValidator<UserLoginReq>
    {
        public UserLoginReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.UserMessage.EmailIsRequired)
                .EmailAddress().WithMessage(Message.UserMessage.InvalidEmail);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Message.UserMessage.PasswordCanNotEmpty);
        }
    }
}
