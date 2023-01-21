using System;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace CorresApp.ViewModel
{
    public class LoginViewModel:ViewModelBase
    {
        public DelegateCommand SHowHidePAsswordCommand
        {
            get
            {
                return new DelegateCommand( () =>
                {
                    IsShowPassword =IsShowPassword?false:true ;
                });
            }
        }

        bool isShowPassword = false;
        public bool IsShowPassword { get { return isShowPassword; } set { isShowPassword = value; RaisePropertyChanged(); } }
        public LoginViewModel(INavigationService navigationServices, IPageDialogService pageDialogServices) : base(navigationServices, pageDialogServices)
        {
        }
    }
}
