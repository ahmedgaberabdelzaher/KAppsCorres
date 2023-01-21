using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CorresApp.Model;
using CorresApp.Services.Interface;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace CorresApp.ViewModel
{
    public class NotificationViewModel:ViewModelBase
    {
        ObservableCollection<Alert> notificationsList = new ObservableCollection<Alert>();

        public ObservableCollection<Alert> NotificationsList { get { return notificationsList; } set { notificationsList = value; RaisePropertyChanged(); } }
        INotificationServices _notificationServices;
       Alert selectedNotification;

        public Alert SelectedNotification { get { return selectedNotification; } set { selectedNotification = value; RaisePropertyChanged(); } }
            public NotificationViewModel(INotificationServices notificationServices,INavigationService navigationServices, IPageDialogService pageDialogServices) : base(navigationServices, pageDialogServices)
        {
            _notificationServices = notificationServices;
        }
        public DelegateCommand NavigateToDetailCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    if (SelectedNotification != null)
                    {
                        Preferences.Set("TaskId", SelectedNotification.TaskId);
                        Preferences.Set("CorrespondenceId", SelectedNotification.Mode==1? SelectedNotification.MeetingId: SelectedNotification.CorrespondenceId);
                        Preferences.Set("RequestType", SelectedNotification.Mode);
                        if (SelectedNotification.Mode == 1)
                        {
                            await NavigationService.NavigateAsync("Meetings");
                        }
                        else
                            await NavigationService.NavigateAsync("Details");
                        SelectedNotification = null;
                    }
                });
            }
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            await GetNotificationsData();
            base.Initialize(parameters);
        }
        public async Task GetNotificationsData()
        {
            try
            {
                IsLoading = true;
                var model = new HomeNotificationBodyModel() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", "") };
                var resp = await _notificationServices.GetNotificationData(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    var NotData = JsonConvert.DeserializeObject<AlertsResponse>(result);
                    if (NotData.Data != null)
                    {
                        NotificationsList = NotData.Data.Data;
                            //new ObservableCollection<Alert>(NotData.Data.Data.Where(c=> c.Mode != 1));;
                    }
                }

            }
            catch (Exception ex)
            {
                IsLoading = false;
            }
            finally
            {
                IsLoading = false;
            }
        }


    }
}
