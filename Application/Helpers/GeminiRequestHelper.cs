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
        public static async Task<object> BuildInlineImagePart(HttpClient client, string imageUrl, int maxBytes)
        {
            using var resp = await client.GetAsync(imageUrl);
            resp.EnsureSuccessStatusCode();

            var bytes = await resp.Content.ReadAsByteArrayAsync();
            if (bytes.Length > maxBytes)
                throw new Exception($"Image too large ({bytes.Length} bytes)");

            var base64 = Convert.ToBase64String(bytes);
            var contentType = resp.Content.Headers.ContentType?.MediaType ?? "image/jpeg";

            return new
            {
                inlineData = new
                {
                    mimeType = contentType,
                    data = base64
                }
            };
        }
    }
}