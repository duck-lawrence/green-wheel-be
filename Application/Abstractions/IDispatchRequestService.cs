using Application.Constants;
using Application.Dtos.Common.Response;
using Application.Dtos.Dispatch.Request;
using Application.Dtos.Dispatch.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IDispatchRequestService
    {
        Task<Guid> CreateAsync(Guid adminId, CreateDispatchReq req);

        Task UpdateStatusAsync(Guid currentAdminId,
            Guid currentAdminStationId,
            Guid id,
            UpdateApproveDispatchReq req);

        Task<IEnumerable<DispatchRes>> GetAllAsync(Guid? fromStationId, Guid? toStationId, DispatchRequestStatus? status);

        Task<DispatchRes?> GetByIdAsync(Guid id);
    }
}