using Application.Constants;
using Application.Dtos.Common.Request;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validators.Common
{
    public class UploadImagesReqValidator : AbstractValidator<UploadImagesReq>
    {
        public UploadImagesReqValidator()
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
