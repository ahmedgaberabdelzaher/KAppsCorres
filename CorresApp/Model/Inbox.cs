using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace CorresApp.Model
{
    public class Inbox
    {
       
        public int id { get; set; }
        public int taskId { get; set; }
        public int reqType { get; set; }
        public string reqTypetxt { get; set; }
        public int corrType { get; set; }
        public string refNo { get; set; }
        public string subject { get; set; }
        public string fromEntity { get; set; }
        public string fromEntityEnglish { get; set; }
        public string toEntity { get; set; }
        public string toEntityEnglish { get; set; }
        public string classification { get; set; }
        public string classificationEnglish { get; set; }
        public string date { get; set; }
        public int isView { get; set; }
        public string className { get; set; }
        public int status { get; set; }
        public string statusName { get; set; }
        public string statusNameEn { get; set; }
        public string Status
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return statusName;
                }
                return statusNameEn;
            }
        }
        public string toentity
        {
            get
            {
                if (reqType == 1)
                {
                    return Preferences.Get("UserName", "");
                }
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return toEntity;
                }
                return toEntityEnglish;
            }
        }
        public string fromentity
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return fromEntity;
                }
                return fromEntityEnglish;
            }
        }
        public string destination { get; set; }
        public string type
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return classification;
                }
                return classificationEnglish;
            }
        }
    }

    public class InboxData
    {
        public ObservableCollection<Inbox> data { get; set; }
        public string result { get; set; }
        public string resultMessage { get; set; }
    }


}
