using Application.Constants;
using Application.Dtos.RentalContract.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.RentalContract
{
    public class CreateRentalContractReqValidator : AbstractValidator<CreateRentalContractReq>
    {
        public CreateRentalContractReqValidator()
        {
            RuleFor(x => x.ModelId)
                .NotEqual(Guid.Empty)
                .WithMessage(Message.RentalContractMessage.ModelIdRequired);

            RuleFor(x => x.StationId)
                .NotEqual(Guid.Empty)
                .WithMessage(Message.RentalContractMessage.StationIdRequired);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTimeOffset.UtcNow)
                .WithMessage(Message.RentalContractMessage.StartDateMustBeFuture);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage(Message.RentalContractMessage.EndDateMustBeAfterStart);

            RuleFor(x => x.Notes)
                .MaximumLength(255)
                .WithMessage("rental_contract.notes_too_long")
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }
}
