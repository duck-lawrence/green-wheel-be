using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Constants.Message;

namespace Application.Dtos.DriverLicense.Request
{
    public class CreateDriverLicenseReqValidator : AbstractValidator<CreateDriverLicenseReq>
    {
        private readonly HashSet<string> validClasses = new()
        {
            "B1", "B", "C1", "C", "D1", "D2", "D", "BE", "C1E", "CE", "D1E", "D2E", "DE"
        };

        public CreateDriverLicenseReqValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty().WithMessage(UserMessage.DriverLicenseNotFound)
                .Matches(@"^\d{9,12}$").WithMessage(UserMessage.InvalidDriverLicenseData);

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage(UserMessage.FirstNameIsRequired);

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage(UserMessage.InvalidDriverLicenseData);

            RuleFor(x => x.Sex)
                .Must(s => s == "0" || s == "1").WithMessage(UserMessage.InvalidDriverLicenseData);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage(UserMessage.DateOfBirthIsRequired)
                .Must(BeAValidDate).WithMessage(UserMessage.InvalidDriverLicenseData);

            RuleFor(x => x.ExpiresAt)
                .NotEmpty().WithMessage(UserMessage.InvalidDriverLicenseData)
                .Must(BeAValidDate).WithMessage(UserMessage.InvalidDriverLicenseData);

            RuleFor(x => x.Class)
                .NotEmpty().WithMessage(UserMessage.InvalidDriverLicenseData)
                .Must(c => validClasses.Contains(c.ToUpper())).WithMessage(UserMessage.InvalidDriverLicenseData);
        }

        private bool BeAValidDate(string date)
        {
            return DateTimeOffset.TryParse(date, out _);
        }
    }
}
