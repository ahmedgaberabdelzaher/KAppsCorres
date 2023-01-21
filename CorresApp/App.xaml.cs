using System;
using System.Globalization;
using System.Threading.Tasks;
using CorresApp.Resources;
using CorresApp.Services.Classes;
using CorresApp.Services.Interface;
using CorresApp.View;
using CorresApp.ViewModel;
using Plugin.Multilingual;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorresApp
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

       public static string BaseUrl = "https://tmsapp.westeurope.cloudapp.azure.com:4434/cm/";
      //    public static string BaseUrl = " https://tmsapp.westeurope.cloudapp.azure.com:4434/cma/";
        public static  string defaultLang = "en";
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            var languageId = Preferences.Get("LanguageId", defaultLang);
            var culturee = new CultureInfo(languageId);
            LangaugeResource.Culture = culturee;
            CrossMultilingual.Current.CurrentCultureInfo = culturee;
            var isLoged = Preferences.Get("IsLogedIn", false);
            if (isLoged)
            {
                //
                //
                await NavigationService.NavigateAsync("NavigationPage/DashBoard");
                //await NavigationService.NavigateAsync("NavigationPage/Inbox");

            }
            else
            {
                 await NavigationService.NavigateAsync("NavigationPage/IntroPAge");
                //await NavigationService.NavigateAsync("NavigationPage/Login");


            }

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<IntroPAge, InroViewModel>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<DashBoard, DashBoardViewMOdel>();
            containerRegistry.RegisterForNavigation<Inbox, InBoxViewModel>();
            containerRegistry.RegisterForNavigation<OutBox, InBoxViewModel>();
            containerRegistry.RegisterForNavigation<Meetings, DetailsViewModel>();
            containerRegistry.RegisterForNavigation<MainPage>();

            containerRegistry.RegisterForNavigation<CalenderPage, CalenderViewModel>();
            containerRegistry.RegisterForNavigation<Details, DetailsViewModel>();
            containerRegistry.RegisterForNavigation<Notifications, NotificationViewModel>();
            containerRegistry.Register<IMicrosoftAuthService, MicrosoftAuthService>();
            containerRegistry.Register<INotificationServices, NotificationServices>();

        }
        protected override void OnSleep()
        {
            base.OnSleep();
            //MicrosoftAuthService microsoftAuthService = new MicrosoftAuthService();
            // microsoftAuthService.OnSignOutAsync();
        }

    }
}
