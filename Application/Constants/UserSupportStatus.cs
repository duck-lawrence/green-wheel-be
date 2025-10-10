namespace Application.Constants
{
    public class UserSupportStatus
    {
        public enum SupportStatus
        {
            Pending = 0,
            InProgress = 1,
            Resolved = 2
        }

        public enum SupportType
        {
            Technical = 0,
            Payment = 1,
            Other = 2
        }
    }
}