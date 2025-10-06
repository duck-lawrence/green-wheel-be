using Application.Abstractions;
using Application.AppExceptions;
using Application.Constants;
using Application.Repositories;
using Domain.Entities;

namespace Application
{
    public class CitizenIdentityService : ICitizenIdentityService
    {
        private readonly IGeminiService _geminiService;
        private readonly ICitizenIdentityRepository _citizenRepo;

        public CitizenIdentityService(
            IGeminiService geminiService,
            ICitizenIdentityRepository citizenRepo)
        {
            _geminiService = geminiService;
            _citizenRepo = citizenRepo;
        }

        public async Task<CitizenIdentity> AddAsync(CitizenIdentity identity)
        {
            identity.CreatedAt = DateTimeOffset.UtcNow;
            identity.UpdatedAt = DateTimeOffset.UtcNow;
            await _citizenRepo.AddAsync(identity);
            return identity;
        }

        public async Task<CitizenIdentity?> GetByIdAsync(Guid id)
            => await _citizenRepo.GetByIdAsync(id);

        public async Task<CitizenIdentity?> GetByIdentityNumberAsync(string identityNumber)
        {
            var citizenIdentity = await _citizenRepo.GetIdNumberAsync(identityNumber);
            if(citizenIdentity == null)
            {
                throw new NotFoundException(Message.CitizenIdentityMessage.CitizenIdentityNotFound);
            }
            return citizenIdentity;
        }
           

        public async Task<CitizenIdentity?> GetByUserId(Guid userId)
            => await _citizenRepo.GetByUserIdAsync(userId);

        public async Task<CitizenIdentity?> ProcessCitizenIdentityAsync(Guid userId, string imageUrl)
        {
            var dto = await _geminiService.ExtractCitizenIdAsync(imageUrl);
            if (dto == null)
                throw new BusinessException(Message.LicensesMessage.InvalidLicenseData);

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
            }
            else
            {
                await _citizenRepo.AddAsync(entity);
            }

            return entity;
        }

        public async Task<bool> RemoveAsync(Guid userId)
        {
            var existing = await _citizenRepo.GetByUserIdAsync(userId);
            if (existing == null)
                throw new NotFoundException(Message.UserMessage.UserNotFound);

            await _citizenRepo.DeleteAsync(existing.Id);
            return true;
        }

        public async Task<CitizenIdentity?> UpdateAsync(CitizenIdentity identity)
        {
            var existing = await _citizenRepo.GetByIdAsync(identity.Id);
            if (existing == null)
                throw new NotFoundException(Message.UserMessage.UserNotFound);

            identity.UpdatedAt = DateTimeOffset.UtcNow;
            await _citizenRepo.UpdateAsync(identity);
            return identity;
        }
    }
}