using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CorresApp.Model;
using CorresApp.Resources;
using CorresApp.Services.Classes;
using CorresApp.Services.Interface;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace CorresApp.ViewModel
{
    public class InroViewModel:ViewModelBase
    {
        public DelegateCommand NavigateToLoginCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    //await NavigationService.NavigateAsync("../Login");
                  await  SignInAsync();
                });
            }
        }
        private readonly IMicrosoftAuthService _microsoftAuthService;
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; RaisePropertyChanged(); }
        }

        public async Task SignInAsync()
        {
            try
            {
                this.IsLoading = true;
                this.User = await _microsoftAuthService.OnSignInAsync();
                var userSites =await GetUserSites();
               if (User!=null&&userSites.Count>0)
                {
                    //Preferences.Set("Id", User.Id);
                    if (userSites.Count>1)
                    {
                        var sites = userSites.Select(c => c.SiteName).ToArray();
                        var selectedSite =await DialogService.DisplayActionSheetAsync("Select Site",null, "Cancel", sites);
                        if(selectedSite!=null&&selectedSite!="cancel")
                        {
                            Preferences.Set("UserSite",selectedSite);

                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        Preferences.Set("UserSite", userSites[0].SiteUrl);
                    }
                    await NavigationService.NavigateAsync("../DashBoard");
                    Preferences.Set("IsLogedIn", true);
                }
              IsLoading = false;
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                IsLoading = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
        public async Task<ObservableCollection<UserSites>> GetUserSites()
        {
            try
            {
                IsLoading = true;
                var model = new GetUserSitesBody() { Email = Preferences.Get("email", "") };
                var resp = await _notificationServices.GetUserSitesService(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    var userSites = JsonConvert.DeserializeObject<ObservableCollection<UserSites>>(result);
                    if (userSites != null)
                    {
                       return userSites;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                return null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                IsLoading = true;
                await this._microsoftAuthService.OnSignOutAsync();
                // Remove the user on the view just for demo purpose
                this.User = null;
                IsLoading = false;
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        INotificationServices _notificationServices;
        public InroViewModel(INavigationService navigationServices, IMicrosoftAuthService microsoftAuthService, IPageDialogService pageDialogServices,INotificationServices notificationServices):base(navigationServices,pageDialogServices)
        {
            _notificationServices = notificationServices;
            _microsoftAuthService = microsoftAuthService;
        }
    }
}
