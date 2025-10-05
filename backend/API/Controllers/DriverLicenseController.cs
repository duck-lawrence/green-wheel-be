using Application.Abstractions;
using Application.Dtos.Common.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverLicenseController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly IPhotoService _photoService;
        private readonly IDriverLicenseService _driverService;
        private readonly ILogger<DriverLicenseController> _logger;

        public DriverLicenseController(
            IGeminiService geminiService,
            IPhotoService photoService,
            IDriverLicenseService driverService,
            ILogger<DriverLicenseController> logger)
        {
            _geminiService = geminiService;
            _photoService = photoService;
            _driverService = driverService;
            _logger = logger;
        }

        //  Upload + OCR (KHÔNG lưu DB)
        [HttpPost("upload")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadDriverLicense([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadReq = new UploadImageReq { File = file };
            var uploadResult = await _photoService.UploadPhotoAsync(uploadReq, "driver-licenses");
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url))
                return BadRequest("Upload to Cloudinary failed");

            var data = await _geminiService.ExtractDriverLicenseAsync(uploadResult.Url);
            if (data == null)
                return BadRequest("Gemini could not extract driver license data");

            return Ok(data);
        }

        //  Upload + OCR + Save to DB
        [HttpPost("process")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ProcessDriverLicense([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadReq = new UploadImageReq { File = file };
            var uploadResult = await _photoService.UploadPhotoAsync(uploadReq, "driver-licenses");
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url))
                return BadRequest("Upload to Cloudinary failed");

            // Lấy userId từ token
            var userIdClaim = User.FindFirst("sid")?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized("User not found in token");

            var userId = Guid.Parse(userIdClaim);

            var entity = await _driverService.ProcessDriverLicenseAsync(userId, uploadResult.Url);
            if (entity == null)
                return BadRequest("Gemini failed to extract driver license");

            _logger.LogInformation("DriverLicense saved for user {UserId}", userId);

            return Ok(new
            {
                message = "Driver license processed successfully",
                driver_license = new
                {
                    entity.Id,
                    entity.Number,
                    entity.FullName,
                    entity.Nationality,
                    entity.Sex,
                    entity.DateOfBirth,
                    entity.ExpiresAt,
                    entity.Class,
                    entity.ImageUrl,
                    entity.UserId
                }
            });
        }

        // Lấy bằng user trong token
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyDriverLicense()
        {
            var userIdClaim = User.FindFirst("sid")?.Value
                           ?? User.FindFirst("sub")?.Value
                           ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized("User not found in token");

            var userId = Guid.Parse(userIdClaim);
            var entity = await _driverService.GetByUserIdAsync(userId);
            if (entity == null)
                return NotFound("Driver license not found for this user");

            return Ok(new
            {
                id = entity.Id,
                number = entity.Number,
                full_name = entity.FullName,
                nationality = entity.Nationality,
                sex = entity.Sex == 0 ? "Nam" : "Nữ",
                date_of_birth = entity.DateOfBirth.ToString("yyyy-MM-dd"),
                expires_at = entity.ExpiresAt.ToString("yyyy-MM-dd"),
                @class = entity.Class,
                image_url = entity.ImageUrl,
                user_id = entity.UserId
            });
        }

        //Lấy bằng userId tay (cho admin hoặc test)
        [HttpGet("by-user/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetDriverLicenseByUser(Guid userId)
        {
            var entity = await _driverService.GetByUserIdAsync(userId);
            if (entity == null)
                return NotFound("Driver license not found for this user");

            return Ok(new
            {
                id = entity.Id,
                number = entity.Number,
                full_name = entity.FullName,
                nationality = entity.Nationality,
                sex = entity.Sex == 0 ? "Nam" : "Nữ",
                date_of_birth = entity.DateOfBirth.ToString("yyyy-MM-dd"),
                expires_at = entity.ExpiresAt.ToString("yyyy-MM-dd"),
                @class = entity.Class,
                image_url = entity.ImageUrl,
                user_id = entity.UserId
            });
        }
    }
}