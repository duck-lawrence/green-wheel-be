using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.RentalContract.Request
{
    public class ConfirmReqValidator : AbstractValidator<ConfirmReq>
    {
        public ConfirmReqValidator()
        {
            RuleFor(x => x.VehicleStatus)
                .NotNull().WithMessage(Message.RentalContractMessage.InvalidVehicleStatus)
                .InclusiveBetween(0, 2).WithMessage(Message.RentalContractMessage.InvalidVehicleStatus)
                .When(x => x.HasVehicle);
        }
    }
}
