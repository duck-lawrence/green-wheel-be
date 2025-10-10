using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Dtos.VehicleModel.Request
{
    public class UploadModelImagesReq
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}