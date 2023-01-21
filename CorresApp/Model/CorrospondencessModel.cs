using System;
namespace CorresApp.Model
{
    public class CorrospondencessModel
    {
        public string Subject { get; set; }
        public string RefrenceNo { get; set; }
        public string Text { get { return $"{Subject} ( {RefrenceNo} )"; } }
    }
}
