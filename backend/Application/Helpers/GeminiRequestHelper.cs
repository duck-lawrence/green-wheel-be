using System.Net;

namespace Application.Helpers
{
    public static class GeminiRequestHelper
    {
        /// <summary>
        /// Tải ảnh từ một URL và build thành object inline_data (base64 + mime type)
        /// để gửi cho Gemini API.
        /// </summary>
        /// <param name="httpClient">HttpClient đã inject qua DI</param>
        /// <param name="imageUrl">Link ảnh (Cloudinary hoặc public URL)</param>
        /// <param name="maxBytes">Giới hạn dung lượng ảnh (default = 10MB)</param>
        /// <returns>object inline_data (mime_type, data base64)</returns>
        /// <exception cref="ArgumentException">Nếu imageUrl rỗng</exception>
        /// <exception cref="InvalidOperationException">Nếu ảnh quá lớn</exception>
        /// <exception cref="HttpRequestException">Nếu gọi HTTP thất bại</exception>
        public static async Task<object> BuildInlineImagePart(
            HttpClient httpClient,
            string imageUrl,
            int maxBytes = 10_000_000)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("imageUrl is empty", nameof(imageUrl));

            // 1. Gửi request tải ảnh
            var resp = await httpClient.GetAsync(imageUrl);

            // 2. Đảm bảo trả về status code thành công (2xx)
            resp.EnsureSuccessStatusCode();

            // 3. Đọc nội dung ảnh thành byte[]
            var bytes = await resp.Content.ReadAsByteArrayAsync();
            if (bytes.Length > maxBytes)
                throw new InvalidOperationException($"Image too large ({bytes.Length} bytes)");

            // 4. Encode sang base64 để Gemini có thể đọc
            var base64 = Convert.ToBase64String(bytes);

            // 5. Xác định mime type
            string mimeType = "image/jpeg"; // mặc định
            var lower = imageUrl.ToLowerInvariant();

            if (lower.EndsWith(".png"))
                mimeType = "image/png";
            else if (lower.EndsWith(".jpg") || lower.EndsWith(".jpeg"))
                mimeType = "image/jpeg";
            else if (resp.Content.Headers.ContentType != null)
                mimeType = resp.Content.Headers.ContentType.MediaType ?? mimeType;

            // 6. Build object inline_data đúng format Gemini yêu cầu
            return new
            {
                inline_data = new
                {
                    mime_type = mimeType,
                    data = base64
                }
            };
        }
    }
}