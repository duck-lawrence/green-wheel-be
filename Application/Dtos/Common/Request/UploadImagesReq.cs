using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Dtos.Common.Request
{
    public class UploadImagesReq
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}