using System;
using System.Net.Http;
using System.Threading.Tasks;
using CorresApp.Helpers;
using CorresApp.Model;
using CorresApp.Services.Interface;

namespace CorresApp.Services.Classes
{
    public class NotificationServices : INotificationServices
    {
        public async Task<HttpResponseMessage> GetHomeNotifications(HomeNotificationBodyModel model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + "api/services/GetHomeNotification", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> GetCalenderData(HomeNotificationBodyModel model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + "api/services/GetMyCalendar", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> GetDashBoardCounters(HomeNotificationBodyModel model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + "api/services/GetCounterDetails", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> GetInBoxData(HomeNotificationBodyModel model,string endurl)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/services/{endurl}", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> GetCorrsDetailsData(CorrespondenceDetailsBody model)//GetCorrespondenceDetails
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/services/GetCorrespondenceDetails", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> ActionService(ActionBody model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/services/CorrespondenceAcions", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> GetNotificationData(HomeNotificationBodyModel model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/services/GetMyNotification", model).ConfigureAwait(false);
            return response;
        }
        public async Task<HttpResponseMessage> MeetingActionService(MeetingActionModel model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/services/MeetingAcions", model).ConfigureAwait(false);
            return response;
        }

        public async Task<HttpResponseMessage> GetUserSitesService(GetUserSitesBody model)
        {
            var response = await HttpManager.PostAsync(App.BaseUrl + $"api/Services/GetUserSites", model).ConfigureAwait(false);
            return response;
        }
    }
}
