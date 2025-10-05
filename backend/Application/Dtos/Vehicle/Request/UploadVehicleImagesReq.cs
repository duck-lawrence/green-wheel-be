using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Dtos.Vehicle.Request
{
    public class UploadVehicleImagesReq
    {
        [FromForm(Name = "file")]
        public List<IFormFile> Files { get; set; } = new();
    }
}