using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.VehicleChecklistItem.Request
{
    public class UploadImgIteamReq
    {
        public IFormFile File { get; set; }
    }
}