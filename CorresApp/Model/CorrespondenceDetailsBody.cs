using System;
namespace CorresApp.Model
{
    public class CorrespondenceDetailsBody: HomeNotificationBodyModel
    {
        public int CorrespondenceId { get; set; }
        public int TaskId { get; set; }
        public int mode { get; set; }
        public int MeetingId { get; set; }
    }
}
