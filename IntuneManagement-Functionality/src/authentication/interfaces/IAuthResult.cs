using Intune.Management.Functionality.Authentication.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Intune.Management.Functionality.Authentication
{
    public interface IAuthResult
    {
        string AccessToken { get; }
        Exception Exception { get; }
        DateTimeOffset ExpiresOn { get; }
        bool HasException { get; }
        bool IsExpired { get; }
        bool IsUIPromptRequired { get; }
        bool IsValidResult { get; }
        string[] Scopes { get; }
        string UserUniqueId { get; }

        AuthenticationHeaderValue CreateAuthorizationHeader();
        bool TryCreateAuthorizationHeader(out AuthenticationHeaderValue header);
    }
}
