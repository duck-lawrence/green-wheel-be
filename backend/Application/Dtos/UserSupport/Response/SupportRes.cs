namespace Application.Dtos.UserSupport.Response
{
    public class SupportRes
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Reply { get; set; }
        public string Status { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? StaffName { get; set; }
    }
}