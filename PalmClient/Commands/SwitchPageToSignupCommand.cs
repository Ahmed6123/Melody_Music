using PalmClient.Services;
using PalmClient.Stores;
using System;

namespace PalmClient.Commands
{
    class SwitchPageToSignupCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        public SwitchPageToSignupCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateSignup(); // Navigate to the Login page
        }
    }
}
