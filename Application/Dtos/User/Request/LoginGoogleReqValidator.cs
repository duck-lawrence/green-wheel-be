using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class LoginGoogleReqValidator : AbstractValidator<LoginGoogleReq>
    {
        public LoginGoogleReqValidator()
        {
            RuleFor(x => x.Credential)
                .NotEmpty().WithMessage(Message.UserMessage.NotInputEmail);
        }
    }
}
