using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Dtos.CitizenIdentity.Request;
using Application.Dtos.DriverLicense.Request;
using Application.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Application
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiSettings _settings;
        private readonly ILogger<GeminiService> _logger;

        public GeminiService(HttpClient httpClient, IOptions<GeminiSettings> options, ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _logger = logger;

            _httpClient.Timeout = TimeSpan.FromSeconds(_settings.HttpTimeoutSeconds);
        }

        //CCCD
        public async Task<CreateCitizenIdentityReq?> ExtractCitizenIdAsync(string imageUrl)
        {
            var prompt = """
            You are an OCR assistant.
            Extract the fields from this Vietnamese Citizen ID card.
            Return ONLY a valid JSON object with keys:
            {id_number, full_name, nationality, sex, date_of_birth, expires_at}.
            Dates MUST be yyyy-MM-dd.
            If a field is missing, use empty string.
            Do not add any explanation, only JSON.
            """;

            var text = await CallGeminiAndExtractText(imageUrl, prompt);
            if (string.IsNullOrWhiteSpace(text)) return null;

            try
            {
                return JsonSerializer.Deserialize<CreateCitizenIdentityReq>(text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize citizen identity JSON: {Raw}", text);
                return null;
            }
        }

        //  Bằng lái xe
        public async Task<CreateDriverLicenseReq?> ExtractDriverLicenseAsync(string imageUrl)
        {
            var prompt = """
                You are an OCR assistant.
                Extract the fields from this Vietnamese Driver License (Bằng lái xe Việt Nam).
                Return ONLY a valid JSON object with the following keys:
                {Number, FullName, Nationality, Sex, DateOfBirth, ExpiresAt, Class}.
                Dates MUST be yyyy-MM-dd.
                If a field is missing, use empty string.
                Do not add any explanation, only JSON.
                """;

            var text = await CallGeminiAndExtractText(imageUrl, prompt);
            if (string.IsNullOrWhiteSpace(text)) return null;

            try
            {
                return JsonSerializer.Deserialize<CreateDriverLicenseReq>(text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deserialize driver license JSON: {Raw}", text);
                return null;
            }
        }

        // Helper chung gọi Gemini API
        private async Task<string?> CallGeminiAndExtractText(string imageUrl, string prompt)
        {
            object imagePart;
            try
            {
                imagePart = await GeminiRequestHelper.BuildInlineImagePart(
                    _httpClient, imageUrl, _settings.MaxImageBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch/encode image: {Url}", imageUrl);
                return null;
            }

            var request = new
            {
                contents = new[]
                {
                    new {
                        parts = new object[]
                        {
                            new { text = prompt },
                            imagePart
                        }
                    }
                }
            };

            var url = $"{_settings.ApiBaseUrl}/models/{_settings.ModelName}:generateContent?key={_settings.ApiKey}";
            _logger.LogInformation("Gemini Request | Model: {Model}, Url: {Url}", _settings.ModelName, imageUrl);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync(url, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HTTP error while calling Gemini API");
                return null;
            }

            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Gemini API Error {Status}: {Error}", response.StatusCode, raw);
                return null;
            }

            try
            {
                var doc = JsonDocument.Parse(raw);
                string? text = null;

                if (doc.RootElement.TryGetProperty("candidates", out var cands) && cands.GetArrayLength() > 0)
                {
                    var candidate = cands[0];
                    if (candidate.TryGetProperty("content", out var content))
                    {
                        if (content.TryGetProperty("parts", out var parts) && parts.GetArrayLength() > 0)
                        {
                            if (parts[0].TryGetProperty("text", out var t))
                                text = t.GetString();
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    _logger.LogWarning("Gemini returned empty text for {Url}", imageUrl);
                    return null;
                }

                // Clean JSON text
                text = text.Replace("```json", "").Replace("```", "").Trim();

                var match = Regex.Match(text, "{.*}", RegexOptions.Singleline);
                if (match.Success) text = match.Value;

                return text;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing Gemini response: {Response}", raw);
                return null;
            }
        }
    }
}