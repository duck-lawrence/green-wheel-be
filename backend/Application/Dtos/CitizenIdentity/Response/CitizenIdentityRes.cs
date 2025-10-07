namespace Application.Dtos.CitizenIdentity.Response
{
    public class CitizenIdentityRes
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string ExpiresAt { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}