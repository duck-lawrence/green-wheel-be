using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklistItem.Request
{
    public class UploadChecklistItemImageReq
    {
        public Guid ItemId { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}