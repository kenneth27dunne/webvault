using ClientApp.Services.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientApp
{
    /// <summary>
    /// This is our custom implementation of an authentication state provider.  The primary purpose of this is 
    /// to implement the overriden method, which is used internally by .NET to determine the current user identity.
    /// For clarity, and ease of use I put the code here that is used to actually trigger login & logout.
    /// </summary>
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public CustomAuthenticationStateProvider()
        {
        }

        /// <summary>
        /// This method should be called upon a successful user login, and it will store the user's JWT token in SecureStorage.
        /// Upon saving this it will also notify .NET that the authentication state has changed which will enable authenticated views
        /// </summary>
        /// <param name="token">Our JWT to store</param>
        /// <returns></returns>
        public async Task Login(string token)
        {
            //Again, this is what I'm doing, but you could do/store/save anything as part of this process
            await SecureStorage.SetAsync("accounttoken", token);

            //Providing the current identity ifnormation
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// This method should be called to log-off the user from the application, which simply removed the stored token and then
        /// notifies of the change
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            SecureStorage.Remove("accounttoken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userInfo = await SecureStorage.GetAsync("accounttoken");
                if (userInfo != null)
                {
                    var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(userInfo, true);

                    var claimsList = decodedToken.Claims.Select(x => new Claim(x.Key, x.Value?.ToString()));
                    var identity = new ClaimsIdentity(claimsList, "Firebase");
                    var principal = new ClaimsPrincipal(identity);

                    return new AuthenticationState(principal);
                }
            }
            catch (Exception ex)
            {
                //This should be more properly handled
                Console.WriteLine("Request failed:" + ex.ToString());
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }
    }
}
