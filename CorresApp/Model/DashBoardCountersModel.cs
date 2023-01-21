using System;
using Xamarin.Essentials;

namespace CorresApp.Model
{

    public class DashBoardCountersModel
    {
        public int internalCount { get; set; }
        public int inExternalCount { get; set; }
        public int outExternalCount { get; set; }
        public int delayCount { get; set; }
        public string userName { get; set; }
        public string userNameEn { get; set; }

        public string department { get; set; }

        public string departmentEn { get; set; }
        public string Name
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return !String.IsNullOrEmpty(userName)? userName: userNameEn;
                }
                return !String.IsNullOrEmpty(userNameEn) ? userNameEn : userName;
            }
        }
        public string DepartmentName
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return !String.IsNullOrEmpty(department) ? department : departmentEn;
                }
                return !String.IsNullOrEmpty(departmentEn) ? departmentEn : department;
            }
        }
    }
   
public class DashBoardCounters
    {
        public DashBoardCountersModel data { get; set; }
        public string result { get; set; }
        public string resultMessage { get; set; }
    }
}
