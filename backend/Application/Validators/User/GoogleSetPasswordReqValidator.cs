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
    public class GoogleSetPasswordReqValidator : AbstractValidator<GoogleSetPasswordReq>
    {
        public GoogleSetPasswordReqValidator()
        {
            RuleFor(x => x.Password)
              .NotEmpty().WithMessage(Message.User.PasswordCanNotEmpty)
              .MinimumLength(6).WithMessage(Message.User.PasswordTooShort);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(Message.User.ConfirmPasswordIsIncorrect);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(Message.User.DateOfBirthIsRequired)
                .Must(dob =>
                {
                    var today = DateTime.Now;
                    var age = today.Year - dob.Year;
                    if (today.Month < dob.Month ||
                    (today.Month == dob.Month && today.Day < dob.Day))
                    {
                        age--;
                    }
                    return age >= 21;
                }).WithMessage(Message.User.InvalidUserAge);
        }
    }
}
