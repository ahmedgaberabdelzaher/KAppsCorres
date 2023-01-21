using System;
using System.Collections.Generic;
using System.IO;
using CorresApp.ViewModel;
using SignaturePad.Forms;
using Xamarin.Forms;

namespace CorresApp.View
{
    public partial class Details : ContentPage
    {
        public Details()
        {
            InitializeComponent();

        }

      async  void Button_Clicked(System.Object sender, System.EventArgs e)
        {
         
        }

       async void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            try
            {

                var vm = (DetailsViewModel)BindingContext;

                var signedImageStream = await signature.GetImageStreamAsync(
                                 SignatureImageFormat.Png,
                             strokeColor: Color.Black,
                           fillColor: Color.White);
                if (signedImageStream != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        signedImageStream.CopyTo(memoryStream);
                        var byteArray = memoryStream.ToArray();
                        vm.SignatureImageBase64 = Convert.ToBase64String(byteArray);
                        await vm.AddActiona(2);
                    }
                }
            }
            catch(Exception ex) {

            }
        }

        void Button_Clicked_2(System.Object sender, System.EventArgs e)
        {
            signature.Clear();
        }

        void Button_Clicked_3(System.Object sender, System.EventArgs e)
        {
            signature.Clear();
            var vm = (DetailsViewModel)BindingContext;
            vm.HideSignatureCommand.Execute();
        }
    }
}
