using System;
using System.Net.Http;
using System.Threading.Tasks;
using CorresApp.Model;

namespace CorresApp.Services.Interface
{
    public interface INotificationServices
    {
        Task<HttpResponseMessage> GetHomeNotifications(HomeNotificationBodyModel model);
        Task<HttpResponseMessage> GetDashBoardCounters(HomeNotificationBodyModel model);
        Task<HttpResponseMessage> GetInBoxData(HomeNotificationBodyModel model,string endurl);
        Task<HttpResponseMessage> GetCorrsDetailsData(CorrespondenceDetailsBody model);
        Task<HttpResponseMessage> ActionService(ActionBody model);
        Task<HttpResponseMessage> GetCalenderData(HomeNotificationBodyModel model);
        Task<HttpResponseMessage> GetNotificationData(HomeNotificationBodyModel model);
        Task<HttpResponseMessage> MeetingActionService(MeetingActionModel model);
        Task<HttpResponseMessage> GetUserSitesService(GetUserSitesBody model);

    }

}
