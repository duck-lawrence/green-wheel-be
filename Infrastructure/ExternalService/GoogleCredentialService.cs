using Application.Abstractions;
using Application.AppSettingConfigurations;
using Application.Constants;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Infrastructure.ExternalService
{
    public class GoogleCredentialService : IGoogleCredentialService
    {
        public readonly GoogleAuthSettings _googleAuthSettings;
        public GoogleCredentialService(IOptions<GoogleAuthSettings> googleAuthSettings)
        {
            _googleAuthSettings = googleAuthSettings.Value;
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyCredential(string credential)
        {
            var setting = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = [_googleAuthSettings.ClientID]
            };
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, setting);
                return payload;
                
            }catch(Exception ex)
            {
                throw new UnauthorizedAccessException(Message.UserMessage.InvalidToken);
            }
        }
    }
}
