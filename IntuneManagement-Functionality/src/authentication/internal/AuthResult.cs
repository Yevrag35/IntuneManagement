using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Intune.Management.Functionality.Authentication.Internal
{
    internal class AuthResult : IAuthResult
    {
        private AuthenticationResult _backendResult;
        private Exception _exception;
        private string[] _scopes;

        string IAuthResult.AccessToken => _backendResult.AccessToken;
        Exception IAuthResult.Exception => _exception;
        DateTimeOffset IAuthResult.ExpiresOn => _backendResult.ExpiresOn;
        bool IAuthResult.HasException => _exception != null;
        bool IAuthResult.IsExpired => _backendResult.ExpiresOn <= DateTimeOffset.Now;
        bool IAuthResult.IsUIPromptRequired => _exception != null && _exception is MsalUiRequiredException;
        bool IAuthResult.IsValidResult => 
            _backendResult != null && !string.IsNullOrWhiteSpace(_backendResult.AccessToken) && !((IAuthResult)this).IsExpired;

        string[] IAuthResult.Scopes => _scopes;
        string IAuthResult.UserUniqueId => _backendResult.UniqueId;

        internal AuthResult(AuthenticationResult result)
        {
            _backendResult = result;
            if (_backendResult != null)
                _scopes = result.Scopes.ToArray();
        }
        internal AuthResult(Exception authException) => _exception = authException;

        AuthenticationHeaderValue IAuthResult.CreateAuthorizationHeader() => 
            AuthenticationHeaderValue.Parse(_backendResult.CreateAuthorizationHeader());
        bool IAuthResult.TryCreateAuthorizationHeader(out AuthenticationHeaderValue header)
        {
            header = null;
            if (AuthenticationHeaderValue.TryParse(_backendResult.CreateAuthorizationHeader(), out AuthenticationHeaderValue outHeader))
            {
                header = outHeader;
            }
            return header != null;
        }
    }
}
