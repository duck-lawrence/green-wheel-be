using Application.Constants;
using Application.Dtos.Vehicle.Request;
using FluentValidation;
using System;

namespace Application.Validators.Vehicle
{
    public class CreateVehicleReqValidator : AbstractValidator<CreateVehicleReq>
    {
        public CreateVehicleReqValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage(Message.VehicleMessage.LicensePlateRequired)
                .MaximumLength(15).WithMessage(Message.VehicleMessage.LicensePlateIsExist); 
            RuleFor(x => x.ModelId)
                .NotEqual(Guid.Empty).WithMessage(Message.VehicleMessage.ModelIdRequired);

            RuleFor(x => x.StationId)
                .NotEqual(Guid.Empty).WithMessage(Message.VehicleMessage.StationIdRequired);
        }
    }
}
