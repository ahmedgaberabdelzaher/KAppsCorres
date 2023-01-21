/*using DeliveryManagement.Resources;
using DeliveryManagement.ViewModels;
using ImTools;
using Newtonsoft.Json;
using Plugin.Geolocator;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using  Xamarin.Forms.Xaml;

namespace DeliveryManagement.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetail : ContentPage
    {
        //TaskDetailViewModel vm;
        public TaskDetail()
        {
            InitializeComponent();
            CustSignaturePad.ClearLabel.Text = "";
            CustSignaturePad.ClearText = "";
            //   AddMapStyle();
            CustSignaturePad.ClearLabel.Text = "";
            CustSignaturePad.ClearText = "";


        }
        void AddMapStyle()
        {
            var assembly = typeof(TaskDetail).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"DeliveryManagement.MapStyle.json");
            string styleFile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            map.MapStyle = MapStyle.FromJson(styleFile);
            map.IsShowingUser = true;
        }
        double? layoutHeight;
        double layoutBoundsHeight;
        int direction;
        const double layoutPropHeightMax = 0.99;
        const double layoutPropHeightMin = 0.3;
        private void PanGestureRecognizer_PanUpdat(object sender, PanUpdatedEventArgs e)
        {
            layoutHeight = layoutHeight ?? ((sender as Frame).Parent as AbsoluteLayout).Height;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    layoutBoundsHeight = AbsoluteLayout.GetLayoutBounds(sender as Frame).Height;
                    break;
                case GestureStatus.Running:
                    direction = e.TotalY < 0 ? 1 : -1;
                    break;
                case GestureStatus.Completed:
                    if (direction > 0) // snap to max/min, you could use an animation....
                    {
                        AbsoluteLayout.SetLayoutBounds(TaskFrame, new Rectangle(0.5, 1.00, 0.97, layoutPropHeightMax));
                        DirectionIcon.IsVisible = false;
                    }
                    else
                    {
                        try
                        {


                            var vm = (TaskDetailViewModel)BindingContext;
                            Device.BeginInvokeOnMainThread(
                                async () =>
                                {
                                    await vm.DrawRoute(vm.TaskDetail.latitude + "," + vm.TaskDetail.longitude);
                                    AbsoluteLayout.SetLayoutBounds(TaskFrame, new Rectangle(0.5, 1.00, 0.97, layoutPropHeightMin));
                                    DirectionIcon.IsVisible = true;
                                });
                        }
                        catch (Exception exp)
                        {

                        }
                    }
                    break;
            }
        }
        private void CashSignatureandImages()
        {
            var vm = (TaskDetailViewModel)BindingContext;

            if (vm.IsSignatureVisible)
            {
                var signedImageStream = CustSignaturePad.GetImageStreamAsync(
                              SignatureImageFormat.Png,
                          strokeColor: Color.Black,
                        fillColor: Color.White).GetAwaiter().GetResult();
                if (signedImageStream != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        signedImageStream.CopyTo(memoryStream);
                        var byteArray = memoryStream.ToArray();
                        Preferences.Set("SignatureImage", Convert.ToBase64String(byteArray));
                    }

                }
            }
            if (vm.AllResultimg64.Count > 0)
            {
                Preferences.Set("SignatureImage", JsonConvert.SerializeObject(vm.AllResultimg64));
            }
        }


        private void ResetSignature_Clicked(object sender, EventArgs e)
        {
            CustSignaturePad.Clear();
        }

        private async void ConfirmSidnature_Clicked(object sender, EventArgs e)
        {
            var vm = (TaskDetailViewModel)BindingContext;

            var signedImageStream = await CustSignaturePad.GetImageStreamAsync(
                             SignatureImageFormat.Png,
                         strokeColor: Color.Black,
                       fillColor: Color.White);
            vm.IsSignatureVisible = false;
            vm.IsSignatureImageVisible = true;

            if (signedImageStream != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    signedImageStream.CopyTo(memoryStream);
                    var byteArray = memoryStream.ToArray();
                    SignatureImage.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    Preferences.Set("SignatureImage" + vm.TaskDetail.id, Convert.ToBase64String(byteArray));
                }

            }
        }


        protected override bool OnBackButtonPressed()
        {
            var gpsLocator = CrossGeolocator.Current;

            if (!gpsLocator.IsGeolocationEnabled)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("", AppResource.GpsNotEnabledMsg, AppResource.OK);
                });
                return true;

            }
            var taskDetailViewModel = (TaskDetailViewModel)BindingContext;
            // taskDetailViewModel.CheckMatchPasswordsValidation();

            if (taskDetailViewModel.IsLoading == true)
            {
                return true;
            }

            else
            { return false; }
        }
        private void Swipe_RightSwiped(object sender, SwipedEventArgs e)
        {
            var vm = (TaskDetailViewModel)BindingContext;
            vm.AcceptanceCommand.Execute("Right");
        }

        private void Swipe_LeftSwiped(object sender, SwipedEventArgs e)
        {
            var vm = (TaskDetailViewModel)BindingContext;
            if (Preferences.Get("LanguageId", "en").Contains("ar"))
            {
                if (e.Direction.ToString() == "Left")
                {
                    vm.AcceptanceCommand.Execute("Right");
                }
                else
                {
                    vm.RejectionCommand.Execute();
                }
            }
            else
            {
                if (e.Direction.ToString() == "Left")
                {
                    vm.RejectionCommand.Execute();
                }
                else
                {
                    vm.AcceptanceCommand.Execute("Right");
                }
            }


        }
    }
}*/