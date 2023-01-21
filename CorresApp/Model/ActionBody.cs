using System;
namespace CorresApp.Model
{
    public class ActionBody
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string CorrespondenceId { get; set; }
        public string TaskId { get; set; }
        public int GoUp { get; set; }
        public string Signature { get; set; }
        public int istxtSignature { get; set; }
        public int ActionId { get; set; }
        public bool inmyRole { get; set; }
        public string Message { get; set; }
    }
}
