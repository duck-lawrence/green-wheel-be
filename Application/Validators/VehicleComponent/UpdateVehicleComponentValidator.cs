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
    public class UpdateVehicleComponentValidator : AbstractValidator<UpdateVehicleComponentReq>
    {
        public UpdateVehicleComponentValidator() 
        {
            RuleFor(x => x.DamageFee)
               .GreaterThan(0).When(x => x.DamageFee.HasValue)
                .WithMessage(Message.VehicleComponentMessage.DamageFeeMustBePositive);
        }
    }
}
