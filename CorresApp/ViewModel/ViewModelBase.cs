using System;
using System.Globalization;
using System.Threading.Tasks;
using CorresApp.Resources;
using CorresApp.Services.Interface;
using Plugin.Multilingual;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CorresApp.ViewModel
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected DelegateCommand NavigateToDetails(int taskid, int id, object SelectedItem)
        {
            return new DelegateCommand(async () =>
            {
                if (SelectedItem != null)
                {
                    Preferences.Set("TaskId", taskid);
                    Preferences.Set("CorrespondenceId", id);

                    await NavigationService.NavigateAsync("Details");
                    SelectedItem = null;
                }

            });
        }
        string userName;
        string deptName;
        public string UserName { get { return Preferences.Get("UserName", ""); } set { userName = value; RaisePropertyChanged(); }}
        public string DeptName { get { return Preferences.Get("DeptName", ""); }set { deptName = value;RaisePropertyChanged(); }}

        public DelegateCommand GoToNotificationCommand
        {
            get
            {
                return new DelegateCommand(async() =>
                {
                    await NavigationService.NavigateAsync("Notifications");
                });
            }
        }

        FlowDirection _FlowDirection;
        public FlowDirection FlowDirection { get { return _FlowDirection; } set { _FlowDirection = value; RaisePropertyChanged(); } }
        string _LanguageId;

        public string LanguageId
        {
            get { return _LanguageId; }
            set
            {
                SetProperty(ref _LanguageId, value);
                RaisePropertyChanged();
            }
        }

        public DelegateCommand GoBackCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await NavigationService.GoBackAsync();
                });
            }
        }

        public DelegateCommand ShowSearchCommand
        {
            get
            {
                return new DelegateCommand( () =>
                {
                    IsSearchVisible = true;
                });
            }
        }

        public DelegateCommand HideSearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    IsSearchVisible = false;
                });
            }
        }

        bool isLoading = false;

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                SetProperty(ref isLoading, value);
            }
        }

        bool iSearchVisible = false;

        public bool IsSearchVisible
        {
            get { return iSearchVisible; }
            set
            {
                SetProperty(ref iSearchVisible, value);
            }
        }
        IMicrosoftAuthService _microsoftAuthService;
        public DelegateCommand SignOutCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await _microsoftAuthService.OnSignOutAsync();
                    Preferences.Clear();
                    await NavigationService.NavigateAsync("../IntroPAge");
                });

                //  return NavigateToDetails(SelectedItem==null?0: SelectedItem.taskId, SelectedItem==null?0: SelectedItem.id, SelectedItem);



            }
        }

        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService DialogService { get; private set; }
        public DelegateCommand LogoutCommand { get; set; }
        public DelegateCommand NavigateToHomeCommand { get; set; }
        public DelegateCommand NavigateToInboxCommand { get; set; }
        public DelegateCommand NavigateToOutboxCommand { get; set; }
        public DelegateCommand NavigateToCalenderCommand { get; set; }

        public DelegateCommand<string> SerachCommand  { get; set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public CultureInfo Culture { get; set; }

        public ViewModelBase(INavigationService navigationService, IPageDialogService dialogService)
        {
            Intilize(navigationService, dialogService);
        }
        public ViewModelBase(INavigationService navigationService, IPageDialogService dialogService, IMicrosoftAuthService microsoftAuthService)
        {
            _microsoftAuthService = microsoftAuthService;
            Intilize(navigationService, dialogService);

        }

        private void Intilize(INavigationService navigationService, IPageDialogService dialogService)
        {
            Culture = new CultureInfo(Preferences.Get("LanguageId", "en"));
            if (Culture.Name.Contains("ar"))
            {
                LanguageId = "2";
            }
            else
            {
                LanguageId = "1";

            }
            LangaugeResource.Culture = Culture;
            CrossMultilingual.Current.CurrentCultureInfo = Culture;

            FlowDirection = CrossMultilingual.Current.CurrentCultureInfo.TextInfo.IsRightToLeft ?
             FlowDirection.RightToLeft :
             FlowDirection.LeftToRight;
            DialogService = dialogService;
            NavigationService = navigationService;
            LogoutCommand = new DelegateCommand(async () => { await logOut(); });
            NavigateToHomeCommand = new DelegateCommand(async () => { await NavigateToHome(); });
            NavigateToInboxCommand = new DelegateCommand(async () => { await NavigateToInbox(); });
            NavigateToOutboxCommand = new DelegateCommand(async () => { await NavigateToOutbox(); });
            NavigateToCalenderCommand = new DelegateCommand(async () => { await NavigateToCalender(); });
        }

        private async Task NavigateToHome()
        {
            await NavigationService.NavigateAsync("../DashBoard");
        }

        private async Task NavigateToInbox()
        {
            await NavigationService.NavigateAsync("../Inbox");
        }
        private async Task NavigateToOutbox()
        {
            await NavigationService.NavigateAsync("../OutBox");
        }
        private async Task NavigateToCalender()
        {
            await NavigationService.NavigateAsync("../CalenderPage");
        }
        private async Task logOut()
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Preferences.Remove("Role");
                    await NavigationService.NavigateAsync("../Login");

                });

            }
            catch (Exception ex)
            {

            }

        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
