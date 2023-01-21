using System;
namespace CorresApp.Model
{
    public class WorkFLowMOdel
    {
        public string role { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public int Status { get; set; }
    }
    public enum WorkFLowStatus
    {
        done,InProgress,Pending
    }
}
