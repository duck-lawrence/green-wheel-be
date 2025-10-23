﻿using Application.Dtos.Common.Request;
using Application.Dtos.Common.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRentalContractRepository : IGenericRepository<RentalContract>
    {
        Task<IEnumerable<RentalContract>> GetByCustomerAsync(Guid customerId, int? status = null);
        Task<bool> HasActiveContractAsync(Guid customerId);
        Task<IEnumerable<RentalContract>> GetAllAsync(int? status = null, string? phone = null,
            string? citizenIdentity = null, string? driverLicense = null, Guid? checklistId = null);
        Task<RentalContract?> GetByChecklistIdAsync(Guid id);
        Task<IEnumerable<RentalContract>> GetContractsByVehicleId(Guid vehicleId);
        Task<PageResult<RentalContract>> GetAllByPaginationAsync(int? status = null, string? phone = null, string? citizenIdentityNumber = null, string? driverLicenseNumber = null, Guid? stationId = null, PaginationParams? pagination = null);
        Task<PageResult<RentalContract>> GetMyContractsAsync(Guid customerId, int? status, PaginationParams pagination);

    }
}
