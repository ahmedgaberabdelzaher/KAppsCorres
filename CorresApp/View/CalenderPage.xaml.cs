using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace CorresApp.View
{
    public partial class CalenderPage : ContentPage
    {
        public CalenderPage()
        {
            InitializeComponent();
        }
        protected  override void OnAppearing()
        {
        //lazyView.LoadViewAsync();
            base.OnAppearing();
        }
    }
}
