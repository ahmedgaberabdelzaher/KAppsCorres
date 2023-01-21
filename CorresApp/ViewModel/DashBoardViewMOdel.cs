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
    public class DashBoardViewMOdel:ViewModelBase
    {
        ObservableCollection<Notification> notificationsList = new ObservableCollection<Notification>();
        INotificationServices _notificationServices;
        public ObservableCollection<Notification> NotificationsList { get { return notificationsList; } set { notificationsList = value; RaisePropertyChanged(); } }
        DashBoardCountersModel _DashBoardCounters;
        public DashBoardCountersModel DashBoardCounters { get { return _DashBoardCounters; } set { _DashBoardCounters = value; RaisePropertyChanged(); } }
        Notification selectedItem = null;
        public Notification SelectedItem { get { return selectedItem; } set { selectedItem = value; RaisePropertyChanged(); } }


        //string UserName = Preferences.Get("","");

        IMicrosoftAuthService _microsoftAuthService;
        public DashBoardViewMOdel(INotificationServices notificationServices,INavigationService navigationServices, IPageDialogService pageDialogServices,IMicrosoftAuthService microsoftAuthService) : base(navigationServices, pageDialogServices,microsoftAuthService)
        {
            _notificationServices = notificationServices;
            GetNotificationsData().ConfigureAwait(false);
            GetDashBoardCounters().ConfigureAwait(false);
            _microsoftAuthService = microsoftAuthService;
            _microsoftAuthService.RefreshToken();
        }
        public DelegateCommand NavigateToDetailCommand
        {
            get
            {
                /*
                return new DelegateCommand(async () =>
                {
                   await NavigationService.NavigateAsync("Details");
                });*/
                return new DelegateCommand(async () =>
                {
                    if (SelectedItem != null)
                    {
                        Preferences.Set("TaskId", SelectedItem.taskId);
                        Preferences.Set("CorrespondenceId", SelectedItem.id);
                        Preferences.Set("RequestType", SelectedItem.reqType);
                        if (SelectedItem.reqType == 1)
                        {
                            await NavigationService.NavigateAsync("Meetings");
                        }
                        else
                            await NavigationService.NavigateAsync("Details");
                        SelectedItem = null;
                    }

                });

              //  return NavigateToDetails(SelectedItem==null?0: SelectedItem.taskId, SelectedItem==null?0: SelectedItem.id, SelectedItem);

                

            }
        }
        public DelegateCommand<string> NavigateToINBOXCommand
        {
            get
            {
                /*
                return new DelegateCommand(async () =>
                {
                   await NavigationService.NavigateAsync("Details");
                });*/
                return new DelegateCommand<string>(async (e) =>
                {

                    var navigationParameters = new NavigationParameters();
                    navigationParameters.Add("Id", e);
                       
                            await NavigationService.NavigateAsync("Inbox",navigationParameters);
                  

                });

              //  return NavigateToDetails(SelectedItem==null?0: SelectedItem.taskId, SelectedItem==null?0: SelectedItem.id, SelectedItem);

                

            }
        }

        public async Task GetNotificationsData()
        {
            try
            {
                IsLoading = true;
                var model = new HomeNotificationBodyModel() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", "") };
             var resp=await   _notificationServices.GetHomeNotifications(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result =await resp.Content.ReadAsStringAsync();
                    var Notdata = JsonConvert.DeserializeObject<NotificationData>(result);
                    if (Notdata.data!=null)
                    {
                        NotificationsList = Notdata.data;
                         //   new ObservableCollection<Notification>( Notdata.data.Where(c =>c.reqType!=1));
                    }
                }

             //  await Task.Delay(100);
                /*
                var data = new ObservableCollection<NotificationModel>()
{
     new NotificationModel()
     {
          date=DateTime.Now.AddDays(1),
           title="A new incoming internalt",
           name="tarek ziad"
           ,destination="multi destination",
            Id=1, type="secret"

     },
       new NotificationModel()
     {
          date=DateTime.Now.AddDays(1),
           title="A new incoming internal",
           name="ali ziad"
           ,destination="over destination",
            Id=1,type="urgent"

     },
         new NotificationModel()
     {
          date=DateTime.Now.AddDays(1),
           title="Lorem inpusum is simply dummy text",
           name="hassan ziad"
           ,destination="single destination",
            Id=1,type="secret"

     },
           new NotificationModel()
     {
          date=DateTime.Now.AddMonths(1),
           title="A new incoming internal",
           name="mohamed ziad"
           ,destination="multi destination",
            Id=1,type="secret"

     }
};
                NotificationsList = data;*/
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

        public async Task GetDashBoardCounters()
        {
            try
            {
                IsLoading = true;
                var model = new HomeNotificationBodyModel() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", "") };
                var resp = await _notificationServices.GetDashBoardCounters(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    var CountersData = JsonConvert.DeserializeObject<DashBoardCounters>(result);
                    if (CountersData.data != null)
                    {
                        DashBoardCounters = CountersData.data;
                        Preferences.Set("UserName", CountersData.data.Name);
                        UserName = CountersData.data.Name;
                        Preferences.Set("DeptName", CountersData.data.DepartmentName);
                        DeptName = CountersData.data.DepartmentName;
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
