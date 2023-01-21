using System;
using System.Threading.Tasks;
using CorresApp.Model;

namespace CorresApp.Services.Interface
{
    public interface IMicrosoftAuthService
    {
        void Initialize();
        Task<User> OnSignInAsync();
        Task OnSignOutAsync();
        Task RefreshToken();
    }
}
