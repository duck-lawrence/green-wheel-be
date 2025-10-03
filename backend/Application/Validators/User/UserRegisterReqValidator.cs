using Application.Constants;
using Application.Dtos.User.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class UserRegisterReqValidator : AbstractValidator<UserRegisterReq>
    {
        public UserRegisterReqValidator()
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

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(Message.User.PhoneIsRequired)
                .Matches(@"^(?:\+84|0)(?:3\d|5[6-9]|7\d|8[1-9]|9\d)\d{7}$")
                .WithMessage(Message.User.InvalidPhone);
        }
    }
}
