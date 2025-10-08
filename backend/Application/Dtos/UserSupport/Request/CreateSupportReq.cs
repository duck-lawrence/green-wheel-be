namespace Application.Dtos.UserSupport.Request
{
    public class CreateSupportReq
    {
        public string Title { get; set; } = null;
        public string Description { get; set; } = null;
        public int Type { get; set; }
    }
}