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
    public class InBoxViewModel:ViewModelBase
    {
        public DelegateCommand NavigateToDetailCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    if (SelectedItem != null)
                    {
                        Preferences.Set("TaskId", SelectedItem.taskId);
                        Preferences.Set("CorrespondenceId", SelectedItem.id);
                        Preferences.Set("RequestType", SelectedItem.reqType);
                        if (SelectedItem.reqType==1)
                        {
                            await NavigationService.NavigateAsync("Meetings");
                        }
                        else
                        await NavigationService.NavigateAsync("Details");
                        SelectedItem = null;
                    }

                });

                //   return NavigateToDetails(SelectedItem == null ? 0 : SelectedItem.taskId, SelectedItem == null ? 0 : SelectedItem.id, SelectedItem);
            }
        }


        ObservableCollection<Inbox> inBoxDataLst = new ObservableCollection<Inbox>();
        INotificationServices _notificationServices;
        public ObservableCollection<Inbox> InBoxDataLst { get { return inBoxDataLst; } set { inBoxDataLst = value; RaisePropertyChanged(); } }
        public static ObservableCollection<Inbox> InBoxDataLstData = new ObservableCollection<Inbox>();
        Inbox selectedItem =null;
        public Inbox SelectedItem  { get { return selectedItem; } set { selectedItem = value; RaisePropertyChanged(); } }

        public DateTime FromDateFilter { get; set; } = DateTime.Now;
        public DateTime ToDateFilter { get; set; } = DateTime.Now;

        string _SearchText;
        public string SearchText { get { return _SearchText; } set { _SearchText = value; RaisePropertyChanged(); } }

        public DelegateCommand<string> ViewIncomInternalCommand
        {
            get
            {
                return new DelegateCommand<string>((id) =>
                {
                    Filter(id);
                });

                //   return NavigateToDetails(SelectedItem == null ? 0 : SelectedItem.taskId, SelectedItem == null ? 0 : SelectedItem.id, SelectedItem);
            }
        }


        bool iRefreshing= false;

        public bool IsRefreshing
        {
            get { return iRefreshing; }
            set
            {
                SetProperty(ref iRefreshing, value);
            }
        }

        private void Filter(string id)
        {
            InBoxDataLst = new ObservableCollection<Inbox>(InBoxDataLstData.Where(c => c.corrType == int.Parse(id)));
        }

        public DelegateCommand<string> GetAllDataCommand
        {
            get
            {
                return new DelegateCommand<string>(async(e) =>
                {
                    IsSearchVisible = false;
                   
                        await GetInboxData();

                   
                });

                //   return NavigateToDetails(SelectedItem == null ? 0 : SelectedItem.taskId, SelectedItem == null ? 0 : SelectedItem.id, SelectedItem);
            }
        }

        public InBoxViewModel(INotificationServices notificationServices,INavigationService navigationServices, IPageDialogService pageDialogServices, IMicrosoftAuthService microsoftAuthService) : base(navigationServices, pageDialogServices,microsoftAuthService)
        {
            SerachCommand = new DelegateCommand<string>((e) => { Search(e); });
            _notificationServices = notificationServices;
       //  GetInboxData().ConfigureAwait(false);

        }
        public async override void Initialize(INavigationParameters parameters)
        {
            if (parameters!=null&&parameters.Count>0)
            {
                var id = parameters["Id"].ToString();
                await GetInboxData(true, id);
            }
            else
            {
                await GetInboxData();
            }
            base.Initialize(parameters);
        }
        private void Search(string searchTxt)
        {
            var e = SearchText.ToLower();
            try
            {
                var res = InBoxDataLstData.Where(c => (DateTime.ParseExact(c.date, "dd/MM/yyyy", Culture) >= FromDateFilter && DateTime.ParseExact(c.date, "dd/MM/yyyy", Culture) <= ToDateFilter) && (c.subject.ToLower().Contains(e) || c.classification.ToLower().Contains(e) || c.classificationEnglish.ToLower().Contains(e) || c.fromentity.ToLower().Contains(e) || c.toentity.ToLower().Contains(e)));
                InBoxDataLst = new ObservableCollection<Inbox>(res);
            }
            catch (Exception ex)
            {

            }
;       
        }

        public async Task GetInboxData(bool withfilter=false,string id="")
        {
            try
            {
                IsLoading = true;
                var currentPage = NavigationService.GetNavigationUriPath();
                string endPoint = "";
                if (currentPage.Split('/').Last().Contains("Inbox"))
                {
                    endPoint = "GetMyInbox";
                }
                else
                {
                    endPoint = "GetMyOutbox";
                }
                var model = new HomeNotificationBodyModel() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", "") };
                var resp = await _notificationServices.GetInBoxData(model,endPoint);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    var CountersData = JsonConvert.DeserializeObject<InboxData>(result);
                    if (CountersData.data != null)
                    {
                        if (withfilter==false)
                        {
  InBoxDataLst = CountersData.data;
                          /*  new ObservableCollection<Inbox>(CountersData.data.Where(c
                           => c.reqType != 1));*/
                        InBoxDataLstData = InBoxDataLst;
                        }
                      else
                        {
                            InBoxDataLstData = CountersData.data;
                            Filter(id);
                        }
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
                IsRefreshing = false;
            }
        }
    }
}
