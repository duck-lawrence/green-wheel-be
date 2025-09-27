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
                .NotEmpty().WithMessage(Message.User.EmailIsRequired)
                .EmailAddress().WithMessage(Message.User.InvalidEmail);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(Message.User.PasswordCanNotEmpty)
                .MinimumLength(6).WithMessage(Message.User.PasswordTooShort);

        }
    }
}
