using Intune.Management.Functionality.Authentication.Internal;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Intune.Management.Functionality.Authentication
{
    public class IntuneApplication
    {
        private const string AUTHORIZATION = "Authorization";
        private IPublicClientApplication _app;
        private IAuthResult _mostRecentResult;

        public bool IsAuthenticated => _mostRecentResult != null && _mostRecentResult.IsValidResult;

        public IntuneApplication() => _app = Parameters.NewBuilder().Build();

        public async Task<IAuthResult> AuthenticateAsync(bool isFirstLaunch = false)
        {
            IAuthResult ar = null;
            if (isFirstLaunch)
            {
                ar = await this.AuthenticateInteractivelyAsync().ConfigureAwait(false);
            }
            else
            {
                ar = await this.AuthenticateSilentlyAsync().ConfigureAwait(false);
                if (ar.IsUIPromptRequired)
                    ar = await this.AuthenticateInteractivelyAsync().ConfigureAwait(false);
            }
            if (ar != null)
                _mostRecentResult = ar;

            return ar;
        }
        private async Task<IAuthResult> AuthenticateInteractivelyAsync()
        {
            try
            {
                AuthenticationResult ar = await _app.AcquireTokenInteractive(Parameters.Scope).WithPrompt(Prompt.SelectAccount).ExecuteAsync().ConfigureAwait(false);
                return new AuthResult(ar);
            }
            catch (Exception e)
            {
                return new AuthResult(e);
            }
        }
        private async Task<IAuthResult> AuthenticateSilentlyAsync()
        {
            try
            {
                IEnumerable<IAccount> accounts = await _app.GetAccountsAsync().ConfigureAwait(false);
                AuthenticationResult ar = await _app.AcquireTokenSilent(Parameters.Scope, accounts.FirstOrDefault()).ExecuteAsync().ConfigureAwait(false);
                return new AuthResult(ar);
            }
            catch (Exception e)
            {
                return new AuthResult(e);
            }
        }

        public void UpdateAuthenticationOnClient(ref HttpClient httpClient)
        {
            if (this.IsAuthenticated)
            {
                httpClient.DefaultRequestHeaders.Authorization = _mostRecentResult.CreateAuthorizationHeader();
            }
        }
    }
}
