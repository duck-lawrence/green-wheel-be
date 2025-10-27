using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Dtos.CitizenIdentity.Request
{
    public class UserUpdateIdentityReqValidator : AbstractValidator<UserUpdateIdentityReq>
    {
        public UserUpdateIdentityReqValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Citizen identity number is required.")
                .Matches(@"^\d{9,12}$").WithMessage("Invalid citizen identity number format.");
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");
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
