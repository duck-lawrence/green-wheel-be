using Application.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklistItem.Request
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
                .Must(f => new[] { ".jpg", ".jpeg", ".png" }.Contains(System.IO.Path.GetExtension(f.FileName).ToLower()))
                .WithMessage(Message.CloudinaryMessage.InvalidFileType);
        }
    }
}
