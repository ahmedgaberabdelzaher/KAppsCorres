using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CorresApp.Model
{

    public class Calaender
    {
        public string title { get; set; }
        public DateTime startDate { get; set; }
        public string url { get; set; }
        public string color { get; set; }
        public int correspondenceId { get; set; }
        public int reqType { get; set; }
    }

    public class CalaenderResponse
    {
        public ObservableCollection<Calaender> data { get; set; }
        public int code { get; set; }
        public string result { get; set; }
        public string resultMessage { get; set; }
    }
}
