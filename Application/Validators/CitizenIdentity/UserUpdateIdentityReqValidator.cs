using Application.Constants;
using Application.Dtos.CitizenIdentity.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Validators.CitizenIdentity
{
    public class UserUpdateIdentityReqValidator : AbstractValidator<UserUpdateIdentityReq>
    {
        public UserUpdateIdentityReqValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage(UserMessage.CitizenIdentityNumberIsRequired)
                .Matches(@"^\d{9,12}$").WithMessage(UserMessage.InvalidCitizenIdData);
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(UserMessage.FullNameIsRequired);
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(UserMessage.FirstNameIsRequired);
            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage(UserMessage.InvalidCitizenIdData);

            RuleFor(x => x.Sex)
                .InclusiveBetween(0, 1).WithMessage(UserMessage.InvalidCitizenIdData);

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTimeOffset.UtcNow).WithMessage(UserMessage.InvalidCitizenIdData);

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTimeOffset.UtcNow).WithMessage(UserMessage.InvalidCitizenIdData);
        }
    }
}
