using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CorresApp.Helpers;
using Polly;
using Xamarin.Essentials;
using Prism.Navigation;

using Prism.Ioc;
using CorresApp;
using CorresApp.Resources;

namespace CorresApp.Helpers
{ 
    public static class HttpManager
    {

        public static INavigationService AppNavigationService => (Application.Current as App).Container.Resolve<INavigationService>();

        static App app = Application.Current as App;

        public static async Task<Tuple<T, bool, string>> GetListAsync<T>(string requestUrl) where T : class
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
                    //  client.DefaultRequestHeaders.Add("Authorization",app.CurrentToken);
                    var response = client.GetAsync(requestUrl).GetAwaiter().GetResult();
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            var JsonObject = JsonConvert.DeserializeObject<T>(responseJson);
                            return Tuple.Create(JsonObject, true, "");
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                // await AppNavigationService.NavigateAsync("CustomerApp:///NavigationPage/Login");
                              //  Application.Current.MainPage = new NavigationPage(new Login());
                                Preferences.Remove("UserLogged");


                            });
                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.SessionExpired);
                        }
                        else
                        {

                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerError);
                        }
                    }
                    else
                    {
                        return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerErrorOrNoInternetConnection);
                    }

                }
                else
                {
                    return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.NoInternet);
                }

            }
            catch (System.Exception exp)
            {
                return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, exp.Message);
            }

        }

        public static async Task<Tuple<T, bool, string>> GetAsync<T>(string requestUrl) where T : class

        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
client.DefaultRequestHeaders.Add("Authorization", "bearer" + " " + Preferences.Get("UserToken", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6ImI5YmZlYjgwLTgyYTctNDI2ZS04NmRmLTUyY2IzOWUwZTMxZCIsImVtYWlsIjoiYWRtaW5AbWljcm9zb2Z0LmNvbSIsInJvbGVzIjoiQWRtaW4iLCJleHAiOjIxMDQ5NDQ5ODB9.8gxtEIqQDXC9FpUhdQVvq3ajan_-U3HJck_4pC8cwW4"));
                 //   client.DefaultRequestHeaders.Add("Authorization", "bearer" + " " + "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhaG1kIiwianRpIjoiNWVmYjk4NzItMDA3OS00MTQzLWI4ZDctZTM3MTBmN2NjZGI2IiwiZW1haWwiOiIxMjEyMUB5LmNvbSIsImV4cCI6MjEwMjQxOTQxOH0.4Uf9ZvOj21OsC2RasT2sHSBH-cceXveHLUDriCdLRMI");
                    
                   // client.DefaultRequestHeaders.Add("Accept-Language","ar");
                   client.DefaultRequestHeaders.Add("LanguageId", Preferences.Get("Language", "2"));
                    var lang = Preferences.Get("LanguageId", "ar") == "en" ? "en" : "ar";
                    if (requestUrl.Contains("?"))
                    {
                        //  requestUrl = $"{requestUrl}&culture={lang}";
                        requestUrl =requestUrl+"&"+lang;

                    }
                    else
                    {
                        requestUrl = $"{requestUrl}?culture={lang}";

                    }

                    var response = await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .RetryAsync
                    (
                        retryCount: 3,
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Console.WriteLine($"Trying to fetch remote data...");

                        var resultJson = await client.GetAsync(requestUrl);

                        return resultJson;
                    });

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                var responseJson = await response.Content.ReadAsStringAsync();
                                var JsonObject = JsonConvert.DeserializeObject<T>(responseJson);
                                return Tuple.Create(JsonObject, true, "");

                            }
                            catch (Exception ex)
                            { throw ex; }




                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                // await AppNavigationService.NavigateAsync("CustomerApp:///NavigationPage/Login");
                              //  Application.Current.MainPage = new NavigationPage(new Login());
                                Preferences.Remove("UserLogged");


                            });
                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.SessionExpired);
                        }
                        else
                        {

                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false,LangaugeResource.ServerError);
                        }
                    }
                    else
                    {
                        return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerErrorOrNoInternetConnection);
                    }

                }
                else
                {
                    return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.NoInternet);
                }

            }
            catch (System.Exception exp)
            {
                return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, exp.Message);
            }

        }

        public static async Task<Tuple<T, bool, string>> DeleteAsync<T>(string requestUrl) where T : class

        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "bearer" + " " + Preferences.Get("UserToken", ""));
                    client.DefaultRequestHeaders.Add("Accept-Language", Preferences.Get("LanguageId", "ar"));

                    var response = await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .RetryAsync
                    (
                        retryCount: 3,
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Console.WriteLine($"Trying to fetch remote data...");

                        var resultJson = await client.DeleteAsync(requestUrl);

                        return resultJson;
                    });

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            try
                            {
                                var responseJson = await response.Content.ReadAsStringAsync();
                                var JsonObject = JsonConvert.DeserializeObject<T>(responseJson);
                                return Tuple.Create(JsonObject, true, "");

                            }
                            catch (Exception ex)
                            { throw ex; }

                        }

                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                // await AppNavigationService.NavigateAsync("CustomerApp:///NavigationPage/Login");
                             //   Application.Current.MainPage = new NavigationPage(new Login());


                            });
                            Preferences.Remove("UserLogged");

                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.SessionExpired);
                        }
                        else
                        {

                            return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerError);
                        }
                    }
                    else
                    {
                        return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerErrorOrNoInternetConnection);
                    }

                }
                else
                {
                    return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.NoInternet);
                }

            }
            catch (System.Exception exp)
            {
                return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.ServerErrorOrNoInternetConnection);
            }

        }

        public static async Task<Tuple<bool, string>> GetAsyncString(string requestUrl)
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "bearer" + " " + Preferences.Get("UserToken", ""));

                    var response = await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .RetryAsync
                    (
                        retryCount: 3,
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Console.WriteLine($"Trying to fetch remote data...");

                        var resultJson = await client.GetAsync(requestUrl);

                        return resultJson;
                    });

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {

                            var responseJson = await response.Content.ReadAsStringAsync();
                            return Tuple.Create(true, "");



                        }
                        else
                        {
                            return Tuple.Create(false, LangaugeResource.ServerError);
                        }
                    }
                    else
                    {
                        return Tuple.Create(false, LangaugeResource.ServerErrorOrNoInternetConnection);
                    }

                }
                else
                {
                    return Tuple.Create(false, LangaugeResource.NoInternet);
                }

            }
            catch (System.Exception exp)
            {
                return Tuple.Create(false, LangaugeResource.ServerErrorOrNoInternetConnection);
            }

        }


        static string jobject;
        public static async Task<HttpResponseMessage> PostAsync<T>(string requestUrl, T Data) where T : class
        {

            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
                    if (!requestUrl.Contains("Login") && !requestUrl.Contains("CustomerSignUp") && !requestUrl.Contains("DriverLogin") && !requestUrl.Contains("GuestTempOrder"))
                    {
                        //client.DefaultRequestHeaders.Add("Authorization", "bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjZCN0FDQzUyMDMwNUJGREI0RjcyNTJEQUVCMjE3N0NDMDkxRkFBRTEiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJhM3JNVWdNRnY5dFBjbExhNnlGM3pBa2ZxdUUifQ.eyJuYmYiOjE1ODkxMTYyNDcsImV4cCI6MTU4OTIwMjY0NywiaXNzIjoibnVsbCIsImF1ZCI6WyJudWxsL3Jlc291cmNlcyIsImNhbGxpbmciXSwiY2xpZW50X2lkIjoiY2FsbGluZyIsInN1YiI6ImRiYzY0ZTY4LTFlOWUtNDM4Ny04ZmNjLTJkNjczMmEzMTMwZiIsImF1dGhfdGltZSI6MTU4OTExNjI0NywiaWRwIjoibG9jYWwiLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjRBVk5TRzZITVBTQ1JMU1VOTEw2M1ZDUTQ2VkhPWVQ2Iiwicm9sZSI6IkFnZW50IiwicHJlZmVycmVkX3VzZXJuYW1lIjoib21uaWFEcml2ZXIiLCJuYW1lIjoib21uaWFEcml2ZXIiLCJlbWFpbCI6Im9tbmlhRHJpdmVyQGpqai52diIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwicGhvbmVfbnVtYmVyIjoiMTA5NzAzMDk5OCIsInBob25lX251bWJlcl92ZXJpZmllZCI6ZmFsc2UsIlRlbmFudF9JZCI6IjUxMzg1NDBlLTg2NGYtNDYwNS1iNjFlLTA1MDFjODM4ZDI4OCIsIlVzZXJUeXBlIjoiMCIsInNjb3BlIjpbImNhbGxpbmciXSwiYW1yIjpbInB3ZCJdfQ.jY_4tyLEhcdiAKjkFho2gRZANy_xnaGRZU0em8wReLauNiMI6xUNdiMYsfEOLr7l-qNE9HBXsrR4Bi6fNICPZSJg-rAepMUfRXDZOG9NfNpnQdWZXOLInJo1URUNQ-VVIJRbhALWVvFro2yep_kLj-ChJ_7ybic5BDa9L-9U2ykcb16laP8loomG6bGolEg-YunjRiz99al-mo7eVPh4o0LVqTkOmIbKZD5mrRQpBBVSAKAja1vHH3l-0ddwDF13-DqD16SrpJk1phiz-Gr7hc8phm0Oy1cgYI_4-E-naZZwy3fvRcbZaEfszbcYjKkiyiIlKfIXUGjLIpDPpx9gnQ");
                        //  client.DefaultRequestHeaders.Add("Authorization", "bearer " + " " + Preferences.Get("UserToken", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhaG1kIiwianRpIjoiNWVmYjk4NzItMDA3OS00MTQzLWI4ZDctZTM3MTBmN2NjZGI2IiwiZW1haWwiOiIxMjEyMUB5LmNvbSIsImV4cCI6MjEwMjQxOTQxOH0.4Uf9ZvOj21OsC2RasT2sHSBH-cceXveHLUDriCdLRMI"));
                        //        client.DefaultRequestHeaders.Add("Accept-Language", "ar");
                        //   client.DefaultRequestHeaders.Add("Accept-Language", Preferences.Get("LanguageId", "en"));
                        client.DefaultRequestHeaders.Add("Authorization", "bearer" + " " + Preferences.Get("UserToken", "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6ImI5YmZlYjgwLTgyYTctNDI2ZS04NmRmLTUyY2IzOWUwZTMxZCIsImVtYWlsIjoiYWRtaW5AbWljcm9zb2Z0LmNvbSIsInJvbGVzIjoiQWRtaW4iLCJleHAiOjIxMDQ5NDQ5ODB9.8gxtEIqQDXC9FpUhdQVvq3ajan_-U3HJck_4pC8cwW4"));

                    }
                    // client.DefaultRequestHeaders.Add("Accept-Language", "ar");
                    var lang = Preferences.Get("LanguageId", "ar") == "en" ? "en" : "ar";
                    requestUrl = $"{requestUrl}?culture={lang}";
                    client.DefaultRequestHeaders.Add("Accept-Language", Preferences.Get("LanguageId", "en"));

                    //var JsonObject = JsonConvert.SerializeObject(Data);
                    jobject = JsonConvert.SerializeObject(Data);
                    var JsonObject = jobject;

                    var content = new StringContent(JsonObject, Encoding.UTF8, "application/json");
                    var response = await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .RetryAsync
                    (
                        retryCount: 3,
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () =>
                        {
                            Console.WriteLine($"Trying to fetch remote data...");
                            HttpResponseMessage resultJson = new HttpResponseMessage();
                            if (requestUrl.Contains("ForgetPassword"))
                            {
                                resultJson = await client.PostAsync(requestUrl, null).ConfigureAwait(false);
                            }
                            else
                            {
                                resultJson = await client.PostAsync(requestUrl, content).ConfigureAwait(false);
                            }
                            return resultJson;
                        });
                    // var response = await client.PostAsync(requestUrl, content).ConfigureAwait(false) ;
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            return response;
                        }

                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                // await AppNavigationService.NavigateAsync("CustomerApp:///NavigationPage/Login");
                               // Application.Current.MainPage = new NavigationPage(new Login());


                            });
                            Preferences.Remove("UserLogged");

                            //    return Tuple.Create((T)Activator.CreateInstance(typeof(T)), false, LangaugeResource.SessionExpired);
                            return new HttpResponseMessage() { StatusCode = response.StatusCode, ReasonPhrase = LangaugeResource.SessionExpired };
                        }
                        else
                        {

                            return new HttpResponseMessage() { StatusCode = response.StatusCode, ReasonPhrase = LangaugeResource.ServerError };
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = LangaugeResource.ServerErrorOrNoInternetConnection };
                    }

                }
                else
                {
                    return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = LangaugeResource.NoInternet };
                }

            }
            catch (System.Exception exp)
            {
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = exp.Message};
            }

        }

        public static async Task<HttpResponseMessage> PutAsync<T>(string requestUrl, T Data) where T : class
        {
            try
            {
                if (NetworkCheck.IsInternet())
                {
                    var client = new System.Net.Http.HttpClient();
                    if (!requestUrl.Contains("Authenticate") && !requestUrl.Contains("ForgetPassword"))
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "bearer " + Preferences.Get("UserToken", ""));
                    }
                    client.DefaultRequestHeaders.Add("Accept-Language", Preferences.Get("LanguageId", "ar"));
                    //   client.DefaultRequestHeaders.Add("Authorization", app.CurrentToken);
                    var JsonObject = JsonConvert.SerializeObject(Data);
                    var content = new StringContent(JsonObject, Encoding.UTF8, "application/json");
                    var response = await Policy
                    .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                    .RetryAsync
                    (
                        retryCount: 3,
                        onRetry: (ex, time) =>
                        {
                            Console.WriteLine($"Something went wrong: {ex.Message}, retrying...");
                        }
                    )
                    .ExecuteAsync(async () =>
                    {
                        Console.WriteLine($"Trying to fetch remote data...");
                        HttpResponseMessage resultJson = new HttpResponseMessage();
                        if (requestUrl.Contains("ForgetPassword"))
                        {
                            resultJson = await client.PutAsync(requestUrl, null).ConfigureAwait(false);
                        }
                        else
                        {
                            resultJson = await client.PutAsync(requestUrl, content).ConfigureAwait(false);
                        }
                        return resultJson;
                    });
                    // var response = await client.PostAsync(requestUrl, content).ConfigureAwait(false) ;
                    //           var response = await client.PutAsync(requestUrl, content);
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsStringAsync();
                            return response;
                        }
                        else
                        {
                            return new HttpResponseMessage() { StatusCode = response.StatusCode, ReasonPhrase = LangaugeResource.ServerError };
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = LangaugeResource.ServerErrorOrNoInternetConnection };
                    }

                }
                else
                {
                    return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = LangaugeResource.NoInternet };
                }

            }
            catch (System.Exception exp)
            {
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest, ReasonPhrase = LangaugeResource.ServerErrorOrNoInternetConnection };
            }

        }

    }
}