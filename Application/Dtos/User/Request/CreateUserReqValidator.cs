using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.User.Request
{
    public class CreateUserReqValidator : AbstractValidator<CreateUserReq>
    {
        public CreateUserReqValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Message.UserMessage.EmailIsRequired)
                .EmailAddress().WithMessage(Message.UserMessage.InvalidEmail);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage(Message.UserMessage.PhoneIsRequired)
                .Matches(@"^(0|\+84)([3|5|7|8|9])+([0-9]{8})$")
                .WithMessage(Message.UserMessage.InvalidPhone);

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(Message.UserMessage.FirstNameIsRequired);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(Message.UserMessage.LastNameIsRequired);

            RuleFor(x => x.Sex)
                .InclusiveBetween(0, 1).WithMessage("user.invalid_sex_value");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTimeOffset.UtcNow.AddYears(-16)).WithMessage(Message.UserMessage.InvalidUserAge);

            RuleFor(x => x.StationId)
                .NotNull().WithMessage(Message.UserMessage.StationIdIsRequired)
                .NotEqual(Guid.Empty).WithMessage(Message.UserMessage.StationIdIsRequired);
        }
    }
}
