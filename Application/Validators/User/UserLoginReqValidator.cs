using Application.Constants;
using Application.Dtos.User.Request;
using FluentValidation;

namespace Application.Validators.User
{
    public class UserLoginReqValidator : AbstractValidator<UserLoginReq>
    {
        public UserLoginReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.UserMessage.EmailIsRequired)
                .EmailAddress().WithMessage(Message.UserMessage.InvalidEmail);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Message.UserMessage.PasswordCanNotEmpty)
                .MinimumLength(8).WithMessage(Message.UserMessage.PasswordTooShort);
        }
    }
}