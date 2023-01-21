using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CorresApp.Helpers;
using CorresApp.Model;
using CorresApp.Services.Interface;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Xamarin.Essentials;
namespace CorresApp.Services.Classes
{
    public class MicrosoftAuthService : IMicrosoftAuthService
    {
       // private readonly string ClientID = "8b31b9a8-f3d5-4919-bc1d-7888ce292169";
        private readonly string ClientID = "9c52ece4-893f-4cc3-b1ed-4a529fd01da2";

        // private readonly string[] Scopes = { "User.Read", "MailboxSettings.Read", "https://khapps.sharepoint.com/.default","https://khapps.sharepoint.com/Sites.ReadWrite.All" };
      //  private readonly string[] Scopes = { "https://khapps.sharepoint.com/.default" };
        private readonly string[] Scopes = { "https://arabou.sharepoint.com/.default" };
        private readonly string GraphUrl = "https://graph.microsoft.com/v1.0/me";
      //  private readonly string RedirectUrl = "msauth://coma.appslink.corresapp";
        private readonly string RedirectUrl = "msauth://coma.appslinkAou.corresapp";

        private IPublicClientApplication publicClientApplication;

        public MicrosoftAuthService()
        {
            Initialize();
        }

        public void Initialize()
        {
            try
            {
            this.publicClientApplication = PublicClientApplicationBuilder.Create(ClientID)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                //.WithAuthority("https://login.microsoftonline.com/6ddbb1f4-52eb-43d9-a3b3-8e8722c67327")
              //  .WithRedirectUri($"msal{ClientID}://auth")
                // .WithRedirectUri("msauth://coma.appslink.corresapp.corresapp/ga0RGNYHvNM5d0SLGQfpQWAPGJ8%3D")

              .WithRedirectUri(RedirectUrl)
                .Build();
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// This object is used to know where to display the authentication activity (for Android) or page.
        /// </summary>
        public static object ParentWindow { get; set; }

        public async Task RefreshToken()
        {
            try
            {
                var accounts = await this.publicClientApplication.GetAccountsAsync();
                var firstAccount = accounts.LastOrDefault();
                var authResult = await this.publicClientApplication.AcquireTokenSilent(Scopes, firstAccount)
                    .ExecuteAsync();
                Preferences.Set("token", authResult.AccessToken);
                Preferences.Set("email", authResult.Account.Username);
            }
            catch (Exception ex)
            {

            }
        

        }
        /// <summary>
        /// Signin with your Microsoft account.
        /// </summary>
        public async Task<User> OnSignInAsync()
        {
            User currentUser = null;

            var accounts = await this.publicClientApplication.GetAccountsAsync();
            try
            {
                try
                {

                    var firstAccount = accounts.LastOrDefault();
                    var authResult = await this.publicClientApplication.AcquireTokenSilent(Scopes, firstAccount)
                        .ExecuteAsync();
                    var authResultForGraphScopes = await this.publicClientApplication.AcquireTokenSilent(new[] {  "User.Read"}, firstAccount)
                      .ExecuteAsync();
                    var user = await this.RefreshUserDataAsync(authResultForGraphScopes?.AccessToken).ConfigureAwait(false);
                    currentUser = user;
                 
                    Preferences.Set("token", authResult.AccessToken);
                    Preferences.Set("email", authResult.Account.Username);
                   

                }
                catch (MsalUiRequiredException ex)
                {
                    // the user was not already connected.
                    try
                    {
                        var authResult = await this.publicClientApplication.AcquireTokenInteractive(Scopes)
                                                    .WithParentActivityOrWindow(ParentWindow).WithUseEmbeddedWebView(true)//.
                                                                                                                          . WithAuthority("https://login.microsoftonline.com/arabou.edu.kw")
                                                                                                    // .WithAuthority("https://login.microsoftonline.com/kappshub.com")

                                                    .ExecuteAsync();
                        var account = authResult.Account;
                        var authResultForGraphScopes = await this.publicClientApplication.AcquireTokenSilent(new[] { "User.Read"}, account)
                   .ExecuteAsync();
                        currentUser = await this.RefreshUserDataAsync(authResultForGraphScopes?.AccessToken).ConfigureAwait(false);
                        Preferences.Set("token", authResult.AccessToken);
                        Preferences.Set("email", authResult.Account.Username);
                    }
                    catch (Exception ex2)
                    {
                        // Manage the exception with a logger as you need
                        System.Diagnostics.Debug.WriteLine(ex2.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                // Manage the exception with a logger as you need
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            if (currentUser != null)
            {
                Preferences.Set("LanguageId", currentUser.preferredLanguage!=null ? currentUser.preferredLanguage.Contains("ar")? "ar-EG" : "en" : "en");
                Preferences.Set("UserName", currentUser.DisplayName);
            }
            return currentUser;
        }

        /// <summary>
        /// Sign out with your Microsoft account.
        /// </summary>
        public async Task OnSignOutAsync()
        {
            var accounts = await this.publicClientApplication.GetAccountsAsync();
            try
            {
                while (accounts.Any())
                {
                    await this.publicClientApplication.RemoveAsync(accounts.FirstOrDefault());
                    accounts = await this.publicClientApplication.GetAccountsAsync();
                }
            }
            catch (Exception ex)
            {
                // Manage the exception with a logger as you need
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Refresh user date from the Graph api.
        /// </summary>
        /// <param name="token">The user access token.</param>
        /// <returns>The current user with his associated informations.</returns>
        private async Task<User> RefreshUserDataAsync(string token)
        {
            
            HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, this.GraphUrl);
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            HttpResponseMessage response = await client.SendAsync(message).
                ConfigureAwait(false);
       //  var response = await   HttpManager.GetAsync<HttpResponseMessage>(GraphUrl);
            User currentUser = null;

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                currentUser = JsonConvert.DeserializeObject<User>(json);
            }

            return currentUser;
        }

    }
}
