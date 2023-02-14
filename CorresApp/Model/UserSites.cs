using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CorresApp.Model
{
    public class UserSitesResponse
    {
        public ObservableCollection<UserSites> data { get; set; }
        public int code { get; set; }
        public string result { get; set; }
        public object resultMessage { get; set; }
    }
    public class UserSites
	{
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteUrl { get; set; }
    }
}

