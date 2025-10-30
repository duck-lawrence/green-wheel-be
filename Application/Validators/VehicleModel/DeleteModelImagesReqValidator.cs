using Application.Constants;
using Application.Dtos.VehicleModel.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.VehicleModel
{
    public class DeleteModelImagesReqValidator : AbstractValidator<DeleteModelImagesReq>
    {
        public DeleteModelImagesReqValidator()
        {
            RuleFor(x => x.ImageIds)
                .NotEmpty()
                .WithMessage(Message.VehicleModelMessage.ImageIdsRequired);
        }
    }
}
