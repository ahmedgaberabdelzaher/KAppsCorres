using System;
namespace CorresApp.Model
{
    public class MeetingActionModel
    {

        public string Token { get; set; }
        public string Email { get; set; }
        public string MeetingId { get; set; }
        public string TaskId { get; set; }
        public string Signature { get; set; }
        public int istxtSignature { get; set; }
        public string Message { get; set; }
        public int ActionId { get; set; }
    }
}
