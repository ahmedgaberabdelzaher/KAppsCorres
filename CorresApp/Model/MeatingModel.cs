using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace CorresApp.Model
{

        public  class MeatingDetailsResponse
        {
            public MeatingDetails Data { get; set; }
            public long Code { get; set; }
            public string Result { get; set; }
            public string ResultMessage { get; set; }
        }

        public  class MeatingDetails
        {
            public int Id { get; set; }
            public long Category { get; set; }
            public long CorrespondenceType { get; set; }
            public object CategorytxtAr { get; set; }
            public string CategorytxtEn { get; set; }
            public string RefNo { get; set; }
            public string Subject { get; set; }
            public string Classification { get; set; }
            public string ClassificationEnglish { get; set; }
            public string PdfUrl { get; set; }
            public string WordUrl { get; set; }
            public bool InmyRole { get; set; }
            public int Status { get; set; }
            public int TaskStatus { get; set; }
        public int IsFeedBack{ get; set; }

        public int MeetingUserType { get; set; }
            public string CommitteeTitle { get; set; }
            public string CommitteeEnglishTitle { get; set; }
            public string CommitteeNumber { get; set; }
            public string MeetingDate { get; set; }
            public string MeetingStartTime { get; set; }
            public string MeetingEndTime { get; set; }
            public string MeetingPlace { get; set; }
        public int sendToManager { get; set; }
        public bool IsOnline { get; set; }
            public ObservableCollection<ATtendee> ATtendees { get; set; }
            public List<Reviewer> Reviewers { get; set; }
            public object MeetingLog { get; set; }
            public Destination Destinations { get; set; }
            public CcEmployee CcEmployees { get; set; }
            public List<File> Files { get; set; }
            public RelatedCorrespondence RelatedCorrespondences { get; set; }
            public CorrespondenceLog CorrespondenceLog { get; set; }
            public ObservableCollection<Chart> Chart { get; set; }
            public string Committee
            {
                get
                {
                    if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                    {
                        return CommitteeTitle;
                    }
                    return CommitteeEnglishTitle;
                }
            }
        public string Title { get { return $"{Subject} ( {RefNo} )"; } }
        public string ClassificationName
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Classification;
                }
                return ClassificationEnglish;
            }
        }
    }

        public  class ATtendee
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public long Attend { get; set; }
            public long Review { get; set; }
            public long Order { get; set; }
            public string Attendtxt { get; set; }
            public string AttendtxtEn { get; set; }
        public string Name
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Title;
                }
                return TitleEn;
            }
        }
        public string Attand
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Attendtxt;
                }
                return AttendtxtEn;
            }
        }
    }
        public  class Reviewer
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public string Message { get; set; }
        public string Name
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Title;
                }
                return TitleEn;
            }
        }
    }

    }



