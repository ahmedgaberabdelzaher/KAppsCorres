using System;
namespace CorresApp.Model
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string  title { get; set; }
        public string name { get; set; }
        public string destination { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
    }
}
