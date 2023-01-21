using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace CorresApp.Model
{

    public partial class AlertsResponse
    {
        public Alerts Data { get; set; }
        public long Code { get; set; }
        public string Result { get; set; }
        public string ResultMessage { get; set; }
    }

    public partial class Alerts
    {
        public long Status { get; set; }
        public long Count { get; set; }
        public ObservableCollection<Alert> Data { get; set; }
    }

    public partial class Alert
    {
        public int CorrespondenceId { get; set; }
        public int MeetingId { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string TitleEnglish { get; set; }
        public int Mode { get; set; }
        public DateTime Date { get; set; }
        public string StringDate { get; set; }
          public string titleTxt
        {
            get
            {
                if (Preferences.Get("LanguageId",App.defaultLang).Contains("ar"))
                {
                    return Title;
                }
                return TitleEnglish;
            }
        }
    }



}
