using System;
using System.Collections.Generic;
using System.Globalization;
using CorresApp.Resources;
using Plugin.Multilingual;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CorresApp.View
{
    public partial class DashBoard : ContentPage
    {
        public DashBoard()
        {
            var Culture = new CultureInfo(Preferences.Get("LanguageId", App.defaultLang));
           
            LangaugeResource.Culture = Culture;
            CrossMultilingual.Current.CurrentCultureInfo = Culture;

            FlowDirection = CrossMultilingual.Current.CurrentCultureInfo.TextInfo.IsRightToLeft ?
             FlowDirection.RightToLeft :
             FlowDirection.LeftToRight;
            InitializeComponent();
        }
    }
}
