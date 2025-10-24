using Application.Constants;
using FluentValidation;
using System;

namespace Application.Dtos.Vehicle.Request
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
