using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IGoogleCredentialService
    {
        public Task<GoogleJsonWebSignature.Payload> VerifyCredential(string credential);
    }
}
