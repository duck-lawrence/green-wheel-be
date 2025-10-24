using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class UserRegisterReqValidator : AbstractValidator<UserRegisterReq>
    {
        public UserRegisterReqValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(Message.UserMessage.PhoneIsRequired)
                .Matches(@"^(0|\+84)([3|5|7|8|9])+([0-9]{8})$")
                .WithMessage(Message.UserMessage.InvalidPhone);

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(Message.UserMessage.FirstNameIsRequired);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(Message.UserMessage.LastNameIsRequired);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Message.UserMessage.PasswordCanNotEmpty)
                .MinimumLength(6).WithMessage(Message.UserMessage.PasswordTooShort);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(Message.UserMessage.ConfirmPasswordIsIncorrect);

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTimeOffset.UtcNow.AddYears(-16)).WithMessage(Message.UserMessage.InvalidUserAge);
        }
    }
}
