using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace CorresApp.Model
{
    public class CorresDetailsResponse
    {
            public CorresDetailData Data { get; set; }
            public string Result { get; set; }
            public string ResultMessage { get; set; }
        }

        public partial class CorresDetailData
    {
            public long Id { get; set; }
            public long Category { get; set; }
            public long CorrespondenceType { get; set; }
            public string CategorytxtAr { get; set; }
            public string CategorytxtEn { get; set; }
            public bool inmyRole { get; set; }
            public int taskStatus { get; set; }
            public string RefNo { get; set; }
            public string Subject { get; set; }
            public string Classification { get; set; }
            public string ClassificationEnglish { get; set; }
            public string PdfUrl { get; set; }
            public ObservableCollection<Destination> Destinations { get; set; }
            public ObservableCollection<CcEmployee> CcEmployees { get; set; }
            public ObservableCollection<File> Files { get; set; }
            public ObservableCollection<RelatedCorrespondence> RelatedCorrespondences { get; set; }
            public ObservableCollection<CorrespondenceLog> CorrespondenceLog { get; set; }
            public ObservableCollection<Chart> Chart { get; set; }
        public string Title { get { return $"{Subject} ( {RefNo} )"; } }
        public string ClassificationName
        {
            get
            {
                if (Preferences.Get("LanguageId",App.defaultLang).Contains("ar"))
                {
                    return Classification;
                }
                return ClassificationEnglish;
            }
        }

    }

    public partial class CcEmployee
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public string Department { get; set; }
            public string DepartmentEn { get; set; }
        public string Name { get {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Title;
                }
                return TitleEn;
            } }
        public string DepartmentName { get {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return Department;
                }
                return DepartmentEn;
            } }

    }

    public partial class Chart
        {
        /*
            "id":1,
            "firstLine":"أنشئ بواسطة",
            "firstLineEn":"Created By",
            "secLine":"طارق خليفة",
            "secLineEn":"Tareq Ziad Khalifha",
            "thirdLine":"08/12/2021",
            "thirdLineEn":"08/12/2021",
            "color":"Orange",
            "status":2
         */
        public long Id { get; set; }
            public string FirstLine { get; set; }
            public string FirstLineEn { get; set; }
            public string SecLine { get; set; }
            public string SecLineEn { get; set; }
            public string ThirdLine { get; set; }
            public string ThirdLineEn { get; set; }
            public string Color { get; set; }
            public long Status { get; set; }
        public string role
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return FirstLine;
                }
                return FirstLineEn;
            }
        }
        public string name
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return SecLine;
                }
                return SecLineEn;
            }
        }

        public string date
        {
            get
            {
                if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                {
                    return ThirdLine;
                }
                return ThirdLineEn;
            }
        }
    }

        public partial class CorrespondenceLog
        {
            public string From { get; set; }
            public string FromEn { get; set; }
            public string To { get; set; }
            public string ToEn { get; set; }
            public string Message { get; set; }
        }

        public partial class Destination
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public object Date { get; set; }
            public long RequireReplay { get; set; }
            public long IsReplyComplete { get; set; }
            public ObservableCollection<Instruction> Instruction { get; set; }
            public ObservableCollection<Detail> Details { get; set; }
        public string instructions
        {
            get
            {
                if (Instruction!=null&& Instruction.Count>0)
                {
                    StringBuilder rs=new StringBuilder() ;
                    if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                    {
                        foreach (var item in Instruction)
                        {
                            if (rs.Length>0)
                            {
                                rs.Append(", ").Append(item.Title)
                               ;
                            }
                            else
                            {
                                rs.Append(item.Title);
                            }
                           
                        }
                    }
                    else {
                        foreach (var item in Instruction)
                        {
                            if (rs.Length > 0)
                            {
                                rs.Append(", ").Append(item.TitleEn)
                               ;
                            }
                            else
                            {
                                rs.Append(item.TitleEn);
                            }
                        }
                    }
                    return rs.ToString();
                }
                return "";
            }

        }

        public string destination
        {
            get
            {
                if (Details != null && Details.Count > 0)
                {
                    StringBuilder rs = new StringBuilder();
                    if (Preferences.Get("LanguageId", App.defaultLang).Contains("ar"))
                    {
                        foreach (var item in Details)
                        {
                            if (rs.Length > 0)
                            {
                                rs.Append(", ").Append(item.Title)
                               ;
                            }
                            else
                            {
                                rs.Append(item.Title);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in Details)
                        {
                            if (rs.Length > 0)
                            {
                                rs.Append(", ").Append(item.TitleEn)
                               ;
                            }
                            else
                            {
                                rs.Append(item.TitleEn);
                            }
                        }
                    }
                    return rs.ToString();
                }
                return "";
            }

        }

        public string date { get; set; }
    }

        public partial class Detail
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
            public long UnitId { get; set; }
            public string Unit { get; set; }
            public string UnitEn { get; set; }
            public object IncommingUnit { get; set; }
            public object IncommingUnitEn { get; set; }
        }

        public partial class Instruction
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string TitleEn { get; set; }
        }

        public partial class File
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
        }

        public partial class RelatedCorrespondence
        {
            public string Subject { get; set; }
            public string RefNo { get; set; }
        public string Text { get { return $"{Subject} ( {RefNo} )"; } }
    }
    

}
