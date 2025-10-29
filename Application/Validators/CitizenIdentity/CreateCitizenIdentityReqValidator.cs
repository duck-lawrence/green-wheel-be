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
    public class CreateCitizenIdentityReqValidator : AbstractValidator<CreateCitizenIdentityReq>
    {
        public CreateCitizenIdentityReqValidator() 
        {
            RuleFor(x => x.IdNumber)
                .NotEmpty().WithMessage(UserMessage.CitizenIdentityNotFound)
                .Matches(@"^\d{9,12}$").WithMessage(UserMessage.InvalidCitizenIdData);
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(UserMessage.FirstNameIsRequired);
            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage(UserMessage.InvalidCitizenIdData);
            RuleFor(x => x.Sex)
                .NotEmpty().WithMessage(UserMessage.InvalidCitizenIdData);
            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(UserMessage.InvalidCitizenIdData)
                .Must(dateStr => DateTimeOffset.TryParse(dateStr, out _))
                .WithMessage(UserMessage.InvalidCitizenIdData);
            RuleFor(x => x.ExpiresAt)
                .NotEmpty().WithMessage(UserMessage.InvalidCitizenIdData)
                .Must(dateStr => DateTimeOffset.TryParse(dateStr, out _))
                .WithMessage(UserMessage.InvalidCitizenIdData);
        }
    }
}
