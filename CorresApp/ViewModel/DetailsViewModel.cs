using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CorresApp.Model;
using CorresApp.Resources;
using CorresApp.Services.Interface;
using Newtonsoft.Json;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SignaturePad.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CorresApp.ViewModel
{
    public class DetailsViewModel : ViewModelBase
    {
        string fileURl = "https://officialdoniald.com/testpdf.pdf";
        public string FileURl { get { return fileURl; } set { fileURl = value; RaisePropertyChanged(); } }

        bool isShowSignature;
        public bool IsShowSignature { get { return isShowSignature; } set { isShowSignature = value; RaisePropertyChanged(); } }

   bool isShowReason;
        public bool IsShowReason { get { return isShowReason; } set { isShowReason = value; RaisePropertyChanged(); } }

        string message;
        public string Message { get { return message; } set { message = value; RaisePropertyChanged(); } }

        bool isUsingNameFSignature;
        public bool IsUsingNameFSignature { get { return isUsingNameFSignature; } set { isUsingNameFSignature = value; RaisePropertyChanged(); } }
   bool isGoToNextLevel;
        public bool IsGoToNextLevel { get { return isGoToNextLevel; } set { isGoToNextLevel = value; RaisePropertyChanged(); } }


        bool isSendToMAnger5;
        public bool IsSendToMAnger5 { get { return isSendToMAnger5; } set { isSendToMAnger5 = value; RaisePropertyChanged(); } }

        bool isShowOnlySendToPresidentAction;
        public bool IsShowOnlySendToPresidentAction { get { return isShowOnlySendToPresidentAction; } set { isShowOnlySendToPresidentAction = value; RaisePropertyChanged(); } }

        bool isShowAllActions;
        public bool IsShowllActions { get { return isShowAllActions; } set { isShowAllActions = value; RaisePropertyChanged(); } }

        bool isShoOnlySignAction;
        public bool IsShoOnlySignAction { get { return isShoOnlySignAction; } set { isShoOnlySignAction = value; RaisePropertyChanged(); } }

        public DelegateCommand<string> OpenFileCommand
        {
            get
            {
                return new DelegateCommand<string>(async (e) =>
                {
                    IsLoading = true;
                    try
                    {


                    HttpClient client = new HttpClient();

                        var uri = new Uri((App.BaseUrl + e));

                    Stream content;
                    MemoryStream stream = new MemoryStream();

                    var response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStreamAsync();
                        content.CopyTo(stream);
                    }

                    await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView(Guid.NewGuid() + ".pdf", "application/pdf", stream, PDFOpenContext.InApp);
                    IsLoading = false;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally { IsLoading = false; }
                });
            }
        }
        public DelegateCommand ShowSignatureCommand
        {
            get
            {
                return new DelegateCommand( () =>
                {
                    IsShowSignature = true;
                });
            }
        }

        public DelegateCommand HideSignatureCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    IsShowSignature = false;
                    Message = "";
                });
            }
        }
        public int actionID { get; set; }
        public DelegateCommand<string> ShowReasonCommand
        {
            get
            {
                return new DelegateCommand<string>( (actionId) =>
                {
                    IsShowReason = true;
                    actionID = int.Parse(actionId);
                });
            }
        }

        public DelegateCommand HideReasonCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    IsShowReason = false;
                    Message = "";
                });
            }
        }

        public DelegateCommand ActionCommand
        {
            get
            {
                return new DelegateCommand(async() =>
                {
                    IsLoading = true;
                   /* if (action=="1")
                    {
                       
                      await  GetSignatureImage().ConfigureAwait(false);
                       await AddActiona(1);
                    }
                    else
                    {*/

                        await AddActiona(actionID);
                   // }
                    IsShowReason = false;
                    IsShowSignature = false;
                    IsLoading = false;

                    Message = "";
                });
            }
        }

        ObservableCollection<DestinationEntitiesMOdel> destinationList = new ObservableCollection<DestinationEntitiesMOdel>();
        public ObservableCollection<DestinationEntitiesMOdel> DestinationList { get { return destinationList; } set { destinationList = value; RaisePropertyChanged(); } }

        ObservableCollection<EmployeeModel> employees = new ObservableCollection<EmployeeModel>();
        public ObservableCollection<EmployeeModel> Employees { get { return employees; } set { employees = value; RaisePropertyChanged(); } }

        ObservableCollection<AttatchementsModel> attatchements = new ObservableCollection<AttatchementsModel>();
        public ObservableCollection<AttatchementsModel> Attatchements { get { return attatchements; } set { attatchements = value; RaisePropertyChanged(); } }

        ObservableCollection<CorrospondencessModel> corrospondencesses = new ObservableCollection<CorrospondencessModel>();
        public ObservableCollection<CorrospondencessModel> Corrospondencesses { get { return corrospondencesses; } set { corrospondencesses = value; RaisePropertyChanged(); } }

        ObservableCollection<WorkFLowMOdel> workFLows = new ObservableCollection<WorkFLowMOdel>();
        public ObservableCollection<WorkFLowMOdel> WorkFLows { get { return workFLows; } set { workFLows = value; RaisePropertyChanged(); } }


        CorresDetailData _DetailsLst { get; set; } = new CorresDetailData();
        public CorresDetailData DetailsLst { get { return _DetailsLst; } set { _DetailsLst = value; RaisePropertyChanged(); } }

        MeatingDetails _MeetingData { get; set; } = new MeatingDetails();
        public MeatingDetails   MeetingData { get { return _MeetingData; } set { _MeetingData = value; RaisePropertyChanged(); } }


        INotificationServices _notificationServices;
        IMicrosoftAuthService _microsoftAuthService;
        public DetailsViewModel(INotificationServices notificationServices,INavigationService navigationServices,IMicrosoftAuthService microsoftAuthService, IPageDialogService pageDialogServices) : base(navigationServices, pageDialogServices,microsoftAuthService)
        {
            //GetImageStreamAsync = getImageDelegate;
            _notificationServices  = notificationServices;
             _microsoftAuthService = microsoftAuthService;
           GetDetailsData().ConfigureAwait(false);
        }

        public async override void Initialize(INavigationParameters parameters)
        {
           //await GetDestinationList();
            //await GetEmployees();
            //await GetCorrospondencesses();
           // await GeAttatchements();
          // await GetWorkFlows();
           // await GetDetailsData();
            base.Initialize(parameters);
        }


        public async Task AddActiona(int actionId)
        {
            try
            {
                IsLoading = true;
                // DetailsLst = new CorresDetailData();
                HttpResponseMessage resp;
                var taskId = Preferences.Get("TaskId", 0);
                var reqType = Preferences.Get("RequestType", 0);
                if (reqType == 1)
                {
                    var model = new MeetingActionModel()
                    {
                        Email = Preferences.Get("email", ""),
                        Token = Preferences.Get("token", ""),
                        MeetingId = Preferences.Get("CorrespondenceId", 0).ToString(),
                        TaskId = taskId.ToString(),
                        ActionId = actionId,
                        Signature = IsUsingNameFSignature ? UserName : SignatureImageBase64,
                        Message = Message,
                        istxtSignature = IsUsingNameFSignature ? 0 : 1
                    };

                    resp = await _notificationServices.MeetingActionService(model);
                }
                else
                {

                    var model = new ActionBody()
                    {
                        Email = Preferences.Get("email", ""),
                        Token = Preferences.Get("token", ""),
                        CorrespondenceId = Preferences.Get("CorrespondenceId", 0).ToString(),
                        TaskId = taskId.ToString(),
                        ActionId = actionId,
                        Signature = IsUsingNameFSignature ? UserName : SignatureImageBase64
                        ,
                        inmyRole = DetailsLst.inmyRole,
                        Message = Message,
                        GoUp = IsGoToNextLevel ? 1 : 0
                    };

                     resp = await _notificationServices.ActionService(model);
                }
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    await DialogService.DisplayAlertAsync("",LangaugeResource.ActionCompledMsg, "Ok");
                    await NavigationService.GoBackAsync();
                }
                IsLoading = false;

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
        private IEnumerable<Point> currentSignature;
        private Point[] savedSignature;
        public IEnumerable<Point> CurrentSignature
        {
            get => currentSignature;
            set
            {
                currentSignature = value;
                RaisePropertyChanged();
            }
        }
        public string SignatureImageBase64 { get; set; }
        public Func<SignatureImageFormat, ImageConstructionSettings, Task<Stream>> GetImageStreamAsync { get; }

        private async Task GetSignatureImage()
        {
            var settings = new ImageConstructionSettings
            {
                StrokeColor = Color.Black,
                BackgroundColor = Color.White,
                StrokeWidth = 1f
            };

            using (var bitmap = await GetImageStreamAsync(SignatureImageFormat.Png, settings))
            {
                if (bitmap != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        bitmap.CopyTo(memoryStream);
                        var byteArray = memoryStream.ToArray();
                        SignatureImageBase64 = Convert.ToBase64String(byteArray);
                    }

                }


            }
        }
        public async Task GetDetailsData()
        {
            try
            {
                IsLoading = true;
                // DetailsLst = new CorresDetailData();
                CorrespondenceDetailsBody model;
                var reqType = Preferences.Get("RequestType", 0);
                if (reqType == 1)
                {
                    model = new CorrespondenceDetailsBody() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", ""), MeetingId = Preferences.Get("CorrespondenceId", 0), TaskId = Preferences.Get("TaskId", 0), mode = 1 };
                }
                else
                {
                     model = new CorrespondenceDetailsBody() { Email = Preferences.Get("email", ""), Token = Preferences.Get("token", ""), CorrespondenceId = Preferences.Get("CorrespondenceId", 0), TaskId = Preferences.Get("TaskId", 0), mode = 0 };
                }
                    var resp = await _notificationServices.GetCorrsDetailsData(model);
                if (resp.IsSuccessStatusCode)
                {
                    var result = await resp.Content.ReadAsStringAsync();
                    if (reqType == 1)
                    {
                        var responsemeeting = JsonConvert.DeserializeObject<MeatingDetailsResponse>(result);
                        if (responsemeeting.Data != null)
                        {
                            
                            MeetingData = responsemeeting.Data;
                            if (MeetingData.Status==9&&MeetingData.sendToManager==1)
                            {
                                IsShowOnlySendToPresidentAction = true;
                            }
                           else if ((MeetingData.Status ==2 ||MeetingData.Status==8)&& MeetingData.sendToManager == 1)
                            {
                                IsShowllActions = true;
                            }
                            else if(MeetingData.sendToManager==5)
                            {
                                IsSendToMAnger5 = true;
                            }
                            
                            else
                            {
                                if (MeetingData.IsFeedBack!=1)
                                {
 IsShoOnlySignAction = true;
                                }
                               
                            }
                        }
                        return;
                    }
                    var response = JsonConvert.DeserializeObject<CorresDetailsResponse>(result);
                    if (response.Data != null)
                    {
                        DetailsLst = response.Data;

                    }
                }
                IsLoading = false;

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

        public async Task GetWorkFlows()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(100);
                WorkFLows = new ObservableCollection<WorkFLowMOdel>() {
                    new WorkFLowMOdel()
                    {
                        name="Eslam Altohamy",
                        role="President Office"
                       ,date=DateTime.Now,
                        Status=0

                    },
                       new WorkFLowMOdel()
                    {
                        name="Mohamed ali hassan",
                        role="Created By"
                        ,date=DateTime.Now,
                        Status=1
                    },
                          new WorkFLowMOdel()
                    {
                        name="Hassan ali ahmed",
                        role="Approved By"
                        ,date=DateTime.Now,
                        Status=2
                    },
                };

            }
            catch { }
        }
        public new DelegateCommand SignOutCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await _microsoftAuthService.OnSignOutAsync();
                    Preferences.Clear();
                    await NavigationService.NavigateAsync("../../IntroPAge");
                });

                //  return NavigateToDetails(SelectedItem==null?0: SelectedItem.taskId, SelectedItem==null?0: SelectedItem.id, SelectedItem);



            }
        }


        public async Task GetDestinationList()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(100);
                DestinationList = new ObservableCollection<DestinationEntitiesMOdel>() {
                    new DestinationEntitiesMOdel()
                    {
                        destination="Eslam Altohamy",
                        instructions="Import",
                        date=DateTime.Now.AddDays(2)
                    },
                      new DestinationEntitiesMOdel()
                    {
                        destination="Eslam Altohamy",
                        instructions="Import",
                        date=DateTime.Now.AddDays(2)
                    }
                };
                
                            }
            catch { }
            }


        public async Task GetEmployees()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(100);
                Employees = new ObservableCollection<EmployeeModel>() {
                    new EmployeeModel()
                    {
                        Name="Eslam Altohamy",
                        Department="President Office"
                        
                    },
                       new EmployeeModel()
                    {
                        Name="Mohamed ali hassan",
                        Department="President Office"

                    },
                };

            }
            catch { }
        }

        public async Task GetCorrospondencesses()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(100);
                Corrospondencesses = new ObservableCollection<CorrospondencessModel>() {
                    new CorrospondencessModel()
                    {
                        Subject="Susspundess according to nothing",
                        RefrenceNo="VP-21789-32"

                    },
                       new CorrospondencessModel()
                    {
                        Subject="Susspundess according to nothing",
                         RefrenceNo="VP-21789-32"

                    },
                };

            }
            catch { }
        }

        public async Task GeAttatchements()
        {
            try
            {
                IsLoading = true;

                await Task.Delay(100);
                Attatchements = new ObservableCollection<AttatchementsModel>() {
                    new AttatchementsModel()
                    {
                        fileName="Eslam Altohamydocument.pdf",

                    },
                       new AttatchementsModel()
                    {
                      fileName  ="filenin entity.pdf",

                    },
                };

            }
            catch { }
        }


    }

}
