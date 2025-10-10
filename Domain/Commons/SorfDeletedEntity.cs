namespace Domain.Commons
{
    public abstract class SorfDeletedEntity
    {
        public  DateTimeOffset? DeletedAt { get; set; }
    }
}
