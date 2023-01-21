using System;
namespace CorresApp.Model
{
    public class InBoxModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string destinationType { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }

    }
}
