using Application.Abstractions;
using Application.Dtos.Common.Request;
using Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitizenIdentityController : ControllerBase
    {
        private readonly IGeminiService _geminiService;
        private readonly IPhotoService _photoService;
        private readonly ICitizenIdentityService _citizenService;
        private readonly ILogger<CitizenIdentityController> _logger;

        public CitizenIdentityController(
            IGeminiService geminiService,
            IPhotoService photoService,
            ICitizenIdentityService citizenService,
            ILogger<CitizenIdentityController> logger)
        {
            _geminiService = geminiService;
            _photoService = photoService;
            _citizenService = citizenService;
            _logger = logger;
        }

        /// <summary>
        /// Upload ảnh CCCD → OCR (Gemini) → trả về DTO (KHÔNG lưu DB).
        /// </summary>
        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadCitizenId([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // 1) Upload ảnh lên Cloudinary
            var uploadReq = new UploadImageReq { File = file };
            var uploadResult = await _photoService.UploadPhotoAsync(uploadReq, "citizen-ids");
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url))
                return BadRequest("Upload to Cloudinary failed");

            // 2) Gọi Gemini OCR → DTO
            var citizenData = await _geminiService.ExtractCitizenIdAsync(uploadResult.Url);
            if (citizenData == null)
                return BadRequest("Gemini could not extract citizen ID data");

            return Ok(citizenData); // trả đúng DTO đầy đủ field
        }

        /// <summary>
        /// OCR trực tiếp từ link ảnh public (không cần upload).
        /// </summary>
        [HttpGet("analyze")]
        public async Task<IActionResult> AnalyzeCitizenId([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest("Missing url param");

            var citizenData = await _geminiService.ExtractCitizenIdAsync(url);
            if (citizenData == null)
                return BadRequest("Gemini could not extract citizen ID data");

            return Ok(citizenData);
        }

        /// <summary>
        /// Upload ảnh CCCD → OCR → map Entity → lưu DB → trả Entity đã lưu.
        /// </summary>
        [HttpPost("process")]
        [Authorize]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ProcessCitizenId([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Upload ảnh lên Cloudinary
            var uploadReq = new UploadImageReq { File = file };
            var uploadResult = await _photoService.UploadPhotoAsync(uploadReq, "citizen-ids");
            if (uploadResult == null || string.IsNullOrEmpty(uploadResult.Url))
                return BadRequest("Upload to Cloudinary failed");

            // Lấy userId từ claim (sid, sub, id)
            var userIdClaim = User.FindFirst("sid")?.Value
                              ?? User.FindFirst("sub")?.Value
                              ?? User.FindFirst("id")?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized("User not found in token");

            var userId = Guid.Parse(userIdClaim);

            // Gọi service để OCR + lưu xuống DB
            var entity = await _citizenService.ProcessCitizenIdentityAsync(userId, uploadResult.Url);
            if (entity == null)
                return BadRequest("Gemini could not extract citizen ID data");

            _logger.LogInformation("CitizenIdentity saved for user {UserId}", userId);

            return Ok(new
            {
                message = "Citizen ID processed successfully",
                citizen_identity = new
                {
                    entity.Id,
                    entity.Number,
                    entity.FullName,
                    entity.Nationality,
                    entity.Sex,
                    entity.DateOfBirth,
                    entity.ExpiresAt,
                    entity.ImageUrl,
                    entity.UserId
                }
            });
        }

        //lấy lên đối tượng
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyCitizenIdentity()
        {
            var userIdClaim = User.FindFirst("sid")?.Value
                      ?? User.FindFirst("sub")?.Value
                      ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim)) return BadRequest("User not found in token");
            var userId = Guid.Parse(userIdClaim);
            var entity = await _citizenService.GetByUserId(userId);
            if (entity == null) return BadRequest("Citizen identity not found for this user");
            return Ok(new
            {
                id = entity.Id,
                number = entity.Number,
                full_name = entity.FullName,
                nationality = entity.Nationality,
                sex = entity.Sex == 0 ? "Nam" : "Nữ",
                date_of_birth = entity.DateOfBirth.ToString("yyyy-MM-dd"),
                expires_at = entity.ExpiresAt.ToString("yyyy-MM-dd"),
                image_url = entity.ImageUrl,
                user_id = entity.UserId
            });
        }

        [HttpGet("by-user/{userId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var entity = await _citizenService.GetByUserId(userId);
            if (entity == null) return NotFound("Citizen identity not found for this user");

            return Ok(new
            {
                id = entity.Id,
                number = entity.Number,
                full_name = entity.FullName,
                nationality = entity.Nationality,
                sex = entity.Sex == 0 ? "Nam" : "Nữ",
                date_of_birth = entity.DateOfBirth.ToString("yyyy-MM-dd"),
                expires_at = entity.ExpiresAt.ToString("yyyy-MM-dd"),
                image_url = entity.ImageUrl,
                user_id = entity.UserId
            });
        }
    }
}