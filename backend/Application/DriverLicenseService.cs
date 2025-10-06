using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.DriverLicense.Request;
using Application.Repositories;
using Domain.Entities;

namespace Application
{
    public class DriverLicenseService : IDriverLicenseService
    {
        private readonly IGeminiService _geminiService;
        private readonly IDriverLicenseRepository _licenseRepo;

        public DriverLicenseService(
            IGeminiService geminiService,
            IDriverLicenseRepository licenseRepo)
        {
            _geminiService = geminiService;
            _licenseRepo = licenseRepo;
        }

        public async Task<DriverLicense?> AddAsync(DriverLicense license)
        {
            license.CreatedAt = DateTimeOffset.UtcNow;
            license.UpdatedAt = DateTimeOffset.UtcNow;
            await _licenseRepo.AddAsync(license);
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
                throw new NotFoundException(Message.User.UserNotFound);

            await _licenseRepo.DeleteAsync(existing.Id);
            return true;
        }

        public async Task<DriverLicense?> UpdateAsync(DriverLicense license)
        {
            var existing = await _licenseRepo.GetByIdAsync(license.Id);
            if (existing == null)
                throw new NotFoundException(Message.User.UserNotFound);

            license.UpdatedAt = DateTimeOffset.UtcNow;
            await _licenseRepo.UpdateAsync(license);
            return license;
        }

        public async Task<DriverLicense?> ProcessDriverLicenseAsync(Guid userId, string imageUrl)
        {
            var dto = await _geminiService.ExtractDriverLicenseAsync(imageUrl);
            if (dto == null)
                throw new BusinessException(Message.Licenses.InvalidLicenseData);

            DateTimeOffset.TryParse(dto.DateOfBirth, out var dob);
            DateTimeOffset.TryParse(dto.ExpiresAt, out var exp);

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

            var existing = await _licenseRepo.GetByUserId(userId);
            if (existing != null)
            {
                entity.Id = existing.Id;
                await _licenseRepo.UpdateAsync(entity);
            }
            else
            {
                await _licenseRepo.AddAsync(entity);
            }

            return entity;
        }

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
    }
}