using Application.Constants;
using Application.Dtos.Common.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.VehicleModel
{
    public class UploadModelImagesReqValidator : AbstractValidator<UploadImagesReq>
    {
        public UploadModelImagesReqValidator()
        {
            RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage(Message.CloudinaryMessage.NotFoundObjectInFile);


            RuleForEach(x => x.Files)
            .Must(IsValidFile)
            .WithMessage(Message.CloudinaryMessage.InvalidFileType);
        }


        private bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0) return false;
            var ext = Path.GetExtension(file.FileName).ToLower();
            return new[] { ".jpg", ".jpeg", ".png" }.Contains(ext);
        }
    }
}
