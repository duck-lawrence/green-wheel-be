﻿using Domain.Entities;

namespace Application.Abstractions
{
    public interface ICitizenIdentityService
    {
        Task<CitizenIdentity?> ProcessCitizenIdentityAsync(Guid userId, string imageUrl);

        Task<CitizenIdentity> AddAsync(CitizenIdentity identity);

        Task<bool> RemoveAsync(Guid userId);

        Task<CitizenIdentity?> UpdateAsync(CitizenIdentity identity);

        Task<CitizenIdentity?> GetByIdAsync(Guid id);

        Task<CitizenIdentity?> GetByUserId(Guid userId);

        Task<CitizenIdentity?> GetByIdentityNumberAsync(string identityNumber);
    }
}