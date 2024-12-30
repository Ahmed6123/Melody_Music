using PalmClient.Services;
using PalmClient.Stores;
using System;

namespace PalmClient.Commands
{
    public class SwitchPageToLoginCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public SwitchPageToLoginCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateLogin(); // Navigate to the Login page
        }
    }
}
