namespace Application.AppSettingConfigurations
{
    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        // Model cho AI Studio, ví dụ: "gemini-2.5-flash" hoặc "gemini-2.5-pro"
        public string ModelName { get; set; } = "gemini-2.5-flash";
        public string ApiBaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta";
        public int MaxImageBytes { get; set; } = 10_000_000; // 10MB
        public int HttpTimeoutSeconds { get; set; } = 60;
    }
}
