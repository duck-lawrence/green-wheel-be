using Application.Dtos.StationFeedback.Request;

namespace Application.Abstractions
{
    public interface IStationFeedbackService
    {
        Task<StationFeedbackRes> CreateAsync(StationFeedbackCreateReq req, Guid customerId);

        Task<IEnumerable<StationFeedbackRes>> GetByStationIdAsync(Guid stationId);

        Task<IEnumerable<StationFeedbackRes>> GetByCustomerIdAsync(Guid customerId);

        Task DeleteAsync(Guid id, Guid customerId);
    }
}