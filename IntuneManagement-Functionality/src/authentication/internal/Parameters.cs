using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Intune.Management.Functionality.Authentication.Internal
{
    internal static class Parameters
    {
        private const string AppId = "d1ddf0e4-d672-4dae-b554-9d5bdfd93547";
        private const string AuthUrl = "https://login.microsoftonline.com/common";
        private const string GraphBaseAddress = "https://graph.microsoft.com";
        private const string RedirectLink = "urn:ietf:wg:oauth:2.0:oob";
        private const string SchemaVersion = "v1.0";
        internal static string[] Scope = new string[1]
        {
            GraphBaseAddress + "/.default"
        };

        private static PublicClientApplicationOptions NewClientOptions()
        {
            return new PublicClientApplicationOptions
            {
                AadAuthorityAudience = AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount,
                AzureCloudInstance = AzureCloudInstance.AzurePublic,
                ClientId = AppId,
                RedirectUri = RedirectLink
            };
        }

        internal static PublicClientApplicationBuilder NewBuilder()
        {
            return PublicClientApplicationBuilder.CreateWithApplicationOptions(NewClientOptions());
        }
    }
}
