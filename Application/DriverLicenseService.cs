﻿using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Dtos.DriverLicense.Request;
using Application.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class DriverLicenseService : IDriverLicenseService
    {
        private readonly IGeminiService _geminiService;
        private readonly IDriverLicenseRepository _licenseRepo;
        private readonly IPhotoService _photoService;

        public DriverLicenseService(
            IGeminiService geminiService,
            IDriverLicenseRepository licenseRepo,
            IPhotoService photoService)
        {
            _geminiService = geminiService;
            _licenseRepo = licenseRepo;
            _photoService = photoService;
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
            => await _licenseRepo.GetByUserIdAsync(userId);

        public async Task<DriverLicense?> GetByLicenseNumberAsync(string licenseNumber)
        {
            var license = await _licenseRepo.GetByLicenseNumber(licenseNumber);
            if (license == null)
            {
                throw new NotFoundException(Message.LicensesMessage.LicenseNotFound);
            }
            return license;
        }

        public async Task<bool> DeleteAsync(Guid userId, string publicId)
        {
            var existing = await _licenseRepo.GetByUserIdAsync(userId);
            if (existing == null)
                throw new NotFoundException(Message.UserMessage.UserNotFound);

            await _licenseRepo.DeleteAsync(existing.Id);
            await _photoService.DeletePhotoAsync(publicId);
            return true;
        }

        public async Task<DriverLicense?> UpdateAsync(DriverLicense license)
        {
            var existing = await _licenseRepo.GetByIdAsync(license.Id);
            if (existing == null)
                throw new NotFoundException(Message.UserMessage.UserNotFound);
            if (!Enum.IsDefined(typeof(LicenseClass), license.Class))
                throw new BadHttpRequestException(Message.LicensesMessage.InvalidLicenseData);
            license.UpdatedAt = DateTimeOffset.UtcNow;
            await _licenseRepo.UpdateAsync(license);
            return license;
        }

        public async Task<DriverLicense?> ProcessDriverLicenseAsync(Guid userId, string imageUrl, string publicId)
        {
            var dto = await _geminiService.ExtractDriverLicenseAsync(imageUrl);
            if (dto == null)
                throw new BusinessException(Message.LicensesMessage.InvalidLicenseData);

            // parse ngày
            DateTimeOffset.TryParse(dto.DateOfBirth, out var dob);
            DateTimeOffset.TryParse(dto.ExpiresAt, out var exp);

            // parse sex + class thành int
            var sex = ParseSex(dto.Sex);
            var licenseClass = ParseLicenseClass(dto.Class);

            var entity = new DriverLicense
            {
                UserId = userId,
                Number = dto.Number ?? string.Empty,
                FullName = dto.FullName ?? string.Empty,
                Nationality = dto.Nationality ?? string.Empty,
                Sex = sex,
                DateOfBirth = dob == default ? DateTimeOffset.MinValue : dob,
                ExpiresAt = exp == default ? DateTimeOffset.MinValue : exp,
                ImageUrl = imageUrl,
                ImagePublicId = publicId,
                Class = licenseClass
            };

            var existing = await _licenseRepo.GetByUserIdAsync(userId);
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

        private static int ParseSex(string? sexStr)
        {
            if (string.IsNullOrWhiteSpace(sexStr)) return 0;
            var s = sexStr.Trim().ToLower();
            if (s.Contains("nữ") || s.Contains("female") || s == "f") return 1;
            return 0;
        }

        private static int ParseLicenseClass(string? classString)
        {
            if (string.IsNullOrWhiteSpace(classString))
                throw new BadRequestException(Message.LicensesMessage.LicenseNotFound);

            var normalized = classString.Trim().ToUpper();

            // Thử parse thành enum
            if (Enum.TryParse(typeof(LicenseClass), normalized, ignoreCase: true, out var value)
                && Enum.IsDefined(typeof(LicenseClass), value))
            {
                return (int)value;
            }

            throw new BadRequestException($"Invalid license class: {classString}");
        }
    }
}