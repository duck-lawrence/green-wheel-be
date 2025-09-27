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
    public class UserUpdateReqValidator : AbstractValidator<UserUpdateReq>
    {
        public UserUpdateReqValidator()
        {
            RuleFor(x => x.Phone)
                .Matches(@"^(?:\+84|0)(?:3\d|5[6-9]|7\d|8[1-9]|9\d)\d{7}$")
                .When(x => !string.IsNullOrEmpty(x.Phone))
                .WithMessage(Message.User.InvalidPhone);

            RuleFor(x => x.DateOfBirth)
                .Must(dob =>
                {
                    var today = DateTime.Now;
                    var age = today.Year - dob.Value.Year;
                    if (today.Month < dob.Value.Month ||
                    (today.Month == dob.Value.Month && today.Day < dob.Value.Day))
                    {
                        age--;
                    }
                    return age >= 21;
                })
                .When(x => x.DateOfBirth.HasValue)
                .WithMessage(Message.User.InvalidUserAge);


        }
    }
}
