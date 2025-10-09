namespace Application.Dtos.StationFeedback.Request
{
    public class StationFeedbackCreateReq
    {
        public string? content { get; set; }
        public int Rating { get; set; }
        public Guid StationId { get; set; }
        public Guid CustomerId { get; set; }
    }
}