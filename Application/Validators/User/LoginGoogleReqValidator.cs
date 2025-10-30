using Application.Constants;
using Application.Dtos.User.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
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
