using Application.Abstractions;
using Application.Dtos.DriverLicense.Request;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class DriverLicenseService : IDriverLicenseService
    {
        private readonly IGeminiService _geminiService;
        private readonly IDriverLicenseRepository _licenseRepo;
        private readonly ILogger<DriverLicenseService> _logger;

        public DriverLicenseService(
            IGeminiService geminiService,
            IDriverLicenseRepository licenseRepo,
            ILogger<DriverLicenseService> logger)
        {
            _geminiService = geminiService;
            _licenseRepo = licenseRepo;
            _logger = logger;
        }

        #region CRUD

        public async Task<DriverLicense?> AddAsync(DriverLicense license)
        {
            license.CreatedAt = DateTimeOffset.UtcNow;
            license.UpdatedAt = DateTimeOffset.UtcNow;

            await _licenseRepo.AddAsync(license);
            _logger.LogInformation("Driver license created: {Id}", license.Id);
            return license;
        }

        public async Task<DriverLicense?> GetAsync(Guid id)
            => await _licenseRepo.GetByIdAsync(id);

        public async Task<DriverLicense?> GetByUserIdAsync(Guid userId)
            => await _licenseRepo.GetByUserId(userId);

        public async Task<DriverLicense?> GetByLicenseNumberAsync(string licenseNumber)
            => await _licenseRepo.GetByLicenseNumber(licenseNumber);

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var existing = await _licenseRepo.GetByUserId(userId);
            if (existing == null)
            {
                _logger.LogWarning("Driver license not found for user: {UserId}", userId);
                return false;
            }

            await _licenseRepo.DeleteAsync(existing.Id);
            _logger.LogInformation("Driver license deleted for user: {UserId}", userId);
            return true;
        }

        public async Task<DriverLicense?> UpdateAsync(DriverLicense license)
        {
            var existing = await _licenseRepo.GetByIdAsync(license.Id);
            if (existing == null)
            {
                _logger.LogWarning("Update failed. License not found: {Id}", license.Id);
                return null;
            }

            license.UpdatedAt = DateTimeOffset.UtcNow;
            await _licenseRepo.UpdateAsync(license);
            _logger.LogInformation("Driver license updated: {Id}", license.Id);
            return license;
        }

        #endregion CRUD

        #region OCR Processing

        public async Task<DriverLicense?> ProcessDriverLicenseAsync(Guid userId, string imageUrl)
        {
            try
            {
                //  1. Gọi Gemini OCR
                var dto = await _geminiService.ExtractDriverLicenseAsync(imageUrl);
                if (dto == null)
                {
                    _logger.LogWarning("Gemini OCR returned null for user {UserId}", userId);
                    return null;
                }

                //  2. Parse dữ liệu an toàn
                DateTimeOffset.TryParse(dto.DateOfBirth, out var dob);
                DateTimeOffset.TryParse(dto.ExpiresAt, out var exp);

                //  3. Map DTO → Entity
                var entity = new DriverLicense
                {
                    UserId = userId,
                    Number = dto.Number ?? string.Empty,
                    FullName = dto.FullName ?? string.Empty,
                    Nationality = dto.Nationality ?? string.Empty,
                    Sex = dto.Sex?.ToLower().Contains("male") == true || dto.Sex?.ToLower().Contains("nam") == true ? 0 : 1,
                    DateOfBirth = dob == default ? DateTimeOffset.MinValue : dob,
                    ExpiresAt = exp == default ? DateTimeOffset.MinValue : exp,
                    ImageUrl = imageUrl,
                    ImagePublicId = string.Empty,
                    Class = ParseLicenseClass(dto.Class)
                };

                //  4. Kiểm tra tồn tại → Update hoặc Add
                var existing = await _licenseRepo.GetByUserId(userId);
                if (existing != null)
                {
                    entity.Id = existing.Id;
                    await _licenseRepo.UpdateAsync(entity);
                    _logger.LogInformation("Driver license updated for user {UserId}", userId);
                }
                else
                {
                    await _licenseRepo.AddAsync(entity);
                    _logger.LogInformation("Driver license created for user {UserId}", userId);
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing driver license for user {UserId}", userId);
                return null;
            }
        }

        #endregion OCR Processing

        #region Helper Methods

        private static int ParseLicenseClass(string? classString)
        {
            if (string.IsNullOrWhiteSpace(classString)) return 0;

            return classString.Trim().ToUpper() switch
            {
                "A1" => 1,
                "A2" => 2,
                "B1" => 3,
                "B2" => 4,
                "C" => 5,
                "D" => 6,
                _ => 0
            };
        }

        #endregion Helper Methods
    }
}