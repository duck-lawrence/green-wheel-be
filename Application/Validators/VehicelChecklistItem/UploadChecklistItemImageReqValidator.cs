using Application.Constants;
using Application.Dtos.VehicleChecklistItem.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.VehicelChecklistItem
{
    public class UploadChecklistItemImageReqValidator : AbstractValidator<UploadChecklistItemImageReq>
    {
        public UploadChecklistItemImageReqValidator()
        {
            RuleFor(x => x.ItemId)
                .NotEqual(Guid.Empty).WithMessage(Message.VehicleChecklistItemMessage.NotFound);

            RuleFor(x => x.File)
                .NotNull().WithMessage(Message.CloudinaryMessage.FileRequired)
                .Must(f => f.Length > 0).WithMessage(Message.CloudinaryMessage.FileEmpty)
                .Must(f => new[] { ".jpg", ".jpeg", ".png" }.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage(Message.CloudinaryMessage.InvalidFileType);
        }
    }
}
