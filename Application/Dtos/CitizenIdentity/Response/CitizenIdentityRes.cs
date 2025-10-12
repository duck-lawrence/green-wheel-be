namespace Application.Dtos.CitizenIdentity.Response
{
    public class CitizenIdentityRes
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateTimeOffset DateOfBirth { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}