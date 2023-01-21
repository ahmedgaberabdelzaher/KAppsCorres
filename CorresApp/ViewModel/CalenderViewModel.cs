
using Xamarin.Plugin.Calendar.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using CorresApp.Model;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using CorresApp.Services.Interface;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace CorresApp.ViewModel
{
    public class CalenderViewModel:ViewModelBase
    {
        
        public DelegateCommand<DateTime> DayTappedCommand => new DelegateCommand<DateTime>(async (date) => await DayTapped(date));
        public DelegateCommand SwipeLeftCommand => new DelegateCommand(() => { MonthYear = MonthYear.AddMonths(2); });
        public DelegateCommand SwipeRightCommand => new DelegateCommand(() => { MonthYear = MonthYear.AddMonths(-2); });
        public DelegateCommand SwipeUpCommand => new DelegateCommand(() => { MonthYear = DateTime.Today; });

        public DelegateCommand<object> EventSelectedCommand => new DelegateCommand<object>(async (item) => await ExecuteEventSelectedCommand(item));
        INotificationServices _notificationServices;
        Calaender selectedItem = null;
        public Calaender SelectedItem { get { return selectedItem; } set { selectedItem = value; RaisePropertyChanged(); } }

        ObservableCollection<Calaender> deadLines =new ObservableCollection<Calaender>();
        public ObservableCollection<Calaender> DeadLines { get { return deadLines; } set { deadLines = value; RaisePropertyChanged(); } }
        public static ObservableCollection<Calaender> DeadLinesLst = new ObservableCollection<Calaender>();
        public DelegateCommand SearchCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Search();
                });

                //   return NavigateToDetails(SelectedItem == null ? 0 : SelectedItem.taskId, SelectedItem == null ? 0 : SelectedItem.id, SelectedItem);
            }
        }
        public DelegateCommand GetAllDataCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsSearchVisible = false;

                  await GetCalenderData();


                });

                //   return NavigateToDetails(SelectedItem == null ? 0 : SelectedItem.taskId, SelectedItem == null ? 0 : SelectedItem.id, SelectedItem);
            }
        }

        public DateTime FromDateFilter { get; set; } = DateTime.Now;
        public DateTime ToDateFilter { get; set; } = DateTime.Now;
        string _SearchText;
        public string SearchText { get { return _SearchText; } set { _SearchText = value; RaisePropertyChanged(); } }

        public CalenderViewModel(INotificationServices notificationServices,INavigationService navigationServices, IPageDialogService pageDialogServices, IMicrosoftAuthService microsoftAuthService) : base(navigationServices, pageDialogServices,microsoftAuthService)
        {
            // var dayformat = new CultureInfo("ar-AE").DateTimeFormat;
            _notificationServices = notificationServices;
            // dayformat.DayNames = f;
            Events = new EventCollection();
            CultureC = new CultureInfo(Preferences.Get("LanguageId", App.defaultLang));
            if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
            {

           
            var f = CultureC.DateTimeFormat.DayNames;
            for (int i = 0; i < f.Length; i++)
            {
                f[i] = f[i].Substring(2, 1);
            }
            CultureC.DateTimeFormat.AbbreviatedDayNames = f;

            }
        }

        private void Search()
        {
           
            try
            {
                var res = DeadLinesLst.Where
                    (c => (c.startDate >= FromDateFilter && c.startDate <= ToDateFilter) &&
                    (c.title.ToLower().Contains(SearchText)))
                ;
                DeadLines = new ObservableCollection<Calaender>(res);
            }
            catch (Exception ex)
            {

            }
;
        }


        public async Task<ObservableCollection<Calaender>> GetCalenderData()
        {
            try
            {
                IsLoading = true;
                var model = new HomeNotificationBodyModel() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", "") };
                var resp = await _notificationServices.GetCalenderData(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    var calenderData = JsonConvert.DeserializeObject<CalaenderResponse>(result);
                    if (calenderData.data != null)
                    {
                        DeadLines = calenderData.data;
                        DeadLinesLst = DeadLines;
                       return calenderData.data;
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
            return new ObservableCollection<Calaender>();
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            try
            {

                var data = await GetCalenderData().ConfigureAwait(false);
                var groupByDate = data.GroupBy(c => c.startDate).ToList();
                /* Events = new EventCollection
                 {
                     [DateTime.Now.AddDays(-5)] = new List<NotificationModel>(GenerateEvents(3, "Cool", DateTime.Now.AddDays(-5))),
                     [DateTime.Now.AddDays(-4)] = new List<NotificationModel>(GenerateEvents(5, "Cool", DateTime.Now.AddDays(-4))),
                     [DateTime.Now.AddDays(+3)] = new List<NotificationModel>(GenerateEvents(4, "Cool", DateTime.Now.AddDays(+3))),
                 };*/
                foreach (var item in groupByDate)
                {
                    Events.Add(item.Key, item.ToList());
                }
                SelectedDate = DateTime.Today;
            }
            catch (Exception ex)
            {

            }
        }
        private IEnumerable<NotificationModel> GenerateEvents(int count, string name,DateTime date)
          {
            return Enumerable.Range(1, count).Select(x => new NotificationModel
            {
                name = $"{name} event{x}",
                title = $"This is {name} event{x}'s description!",
                date = date , destination="multi destination",
                type = "urgent"
            });
        }

        public EventCollection Events { get; }

        private DateTime _monthYear = DateTime.Today;
        public DateTime MonthYear
        {
            get => _monthYear;
            set => SetProperty(ref _monthYear, value);
        }

        private DateTime? _selectedDate = DateTime.Today;
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        
        private CultureInfo _culture = CultureInfo.InvariantCulture;
        public CultureInfo CultureC
        {
            get => _culture;
            set => SetProperty(ref _culture, value);
        }
       
        private static async Task DayTapped(DateTime date)
        {
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item != null)
            {
                SelectedItem = (Calaender)item;
                Preferences.Set("TaskId", 0);
                Preferences.Set("CorrespondenceId", SelectedItem.correspondenceId);
                Preferences.Set("RequestType", SelectedItem.reqType);
                if (SelectedItem.reqType == 1)
                {
                    await NavigationService.NavigateAsync("Meetings");
                }
                else
                    await NavigationService.NavigateAsync("Details");
                SelectedItem = null;
            }
        }


        public DelegateCommand NavigateToDetailCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    if (SelectedItem != null)
                    {
                        Preferences.Set("TaskId", 0);
                        Preferences.Set("CorrespondenceId", SelectedItem.correspondenceId);
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
            }
        }






    }
}
