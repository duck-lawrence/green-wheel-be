using Application.Abstractions;
using Application.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class CitizenIdentityService : ICitizenIdentityService
    {
        private readonly IGeminiService _geminiService;
        private readonly ICitizenIdentityRepository _citizenRepo;
        private readonly ILogger<CitizenIdentityService> _logger;

        public CitizenIdentityService(
            IGeminiService geminiService,
            ICitizenIdentityRepository citizenRepo,
            ILogger<CitizenIdentityService> logger)
        {
            _geminiService = geminiService;
            _citizenRepo = citizenRepo;
            _logger = logger;
        }

        public async Task<CitizenIdentity> AddAsync(CitizenIdentity identity)
        {
            identity.CreatedAt = DateTimeOffset.UtcNow;
            identity.UpdatedAt = DateTimeOffset.UtcNow;
            await _citizenRepo.AddAsync(identity);
            _logger.LogInformation("CitizenIdentity created: {Id}", identity.Id);
            return identity;
        }

        public async Task<CitizenIdentity?> GetByIdAsync(Guid id)
            => await _citizenRepo.GetByIdAsync(id);

        public async Task<CitizenIdentity?> GetByIdentityNumberAsync(string identityNumber)
            => await _citizenRepo.GetIdNumberAsync(identityNumber);

        public async Task<CitizenIdentity?> GetByUserId(Guid userId)
            => await _citizenRepo.GetByUserIdAsync(userId);

        public async Task<CitizenIdentity?> ProcessCitizenIdentityAsync(Guid userId, string imageUrl)
        {
            try
            {
                var dto = await _geminiService.ExtractCitizenIdAsync(imageUrl);
                if (dto == null)
                {
                    _logger.LogWarning("Gemini OCR returned null for user {UserId}", userId);
                    return null;
                }

                DateTimeOffset.TryParse(dto.DateOfBirth, out var dob);
                DateTimeOffset.TryParse(dto.ExpiresAt, out var exp);

                var entity = new CitizenIdentity
                {
                    UserId = userId,
                    Number = dto.IdNumber ?? string.Empty,
                    FullName = dto.FullName ?? string.Empty,
                    Nationality = dto.Nationality ?? string.Empty,
                    Sex = dto.Sex?.ToLower().Contains("male") == true || dto.Sex?.ToLower().Contains("nam") == true ? 0 : 1,
                    DateOfBirth = dob == default ? DateTimeOffset.MinValue : dob,
                    ExpiresAt = exp == default ? DateTimeOffset.MinValue : exp,
                    ImageUrl = imageUrl,
                    ImagePublicId = string.Empty
                };

                var existing = await _citizenRepo.GetByUserIdAsync(userId);
                if (existing != null)
                {
                    entity.Id = existing.Id;
                    await _citizenRepo.UpdateAsync(entity);
                    _logger.LogInformation("CitizenIdentity updated for user {UserId}", userId);
                }
                else
                {
                    await _citizenRepo.AddAsync(entity);
                    _logger.LogInformation("CitizenIdentity created for user {UserId}", userId);
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing Citizen Identity for user {UserId}", userId);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(Guid userId)
        {
            var existing = await _citizenRepo.GetByUserIdAsync(userId);
            if (existing == null)
            {
                _logger.LogWarning("CitizenIdentity not found for user {UserId}", userId);
                return false;
            }

            await _citizenRepo.DeleteAsync(existing.Id);
            _logger.LogInformation("CitizenIdentity deleted: {UserId}", userId);
            return true;
        }

        public async Task<CitizenIdentity?> UpdateAsync(CitizenIdentity identity)
        {
            var existing = await _citizenRepo.GetByIdAsync(identity.Id);
            if (existing == null) return null;

            identity.UpdatedAt = DateTimeOffset.UtcNow;
            await _citizenRepo.UpdateAsync(identity);
            _logger.LogInformation("CitizenIdentity updated: {Id}", identity.Id);
            return identity;
        }
    }
}