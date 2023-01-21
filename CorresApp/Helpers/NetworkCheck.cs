using Xamarin.Essentials;

namespace CorresApp.Helpers
{
    public static class NetworkCheck
    {
        public static bool IsInternet()
        {
            if (Connectivity.NetworkAccess==NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}