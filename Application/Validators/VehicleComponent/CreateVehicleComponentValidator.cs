using Application.Constants;
using Application.Dtos.VehicleComponent.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.VehicleComponent
{
    internal class CreateVehicleComponentValidator : AbstractValidator<CreateVehicleComponentReq>
    {
        public CreateVehicleComponentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Message.VehicleComponentMessage.NameIsRequired);
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(Message.VehicleComponentMessage.DescriptionIsRequired);
            RuleFor(x => x.DamageFee)
                .NotEmpty().WithMessage(Message.VehicleComponentMessage.DamageFeeIsRequired)
                .GreaterThanOrEqualTo(0).WithMessage(Message.VehicleComponentMessage.DamageFeeMustBePositive);
        }
    }
}
