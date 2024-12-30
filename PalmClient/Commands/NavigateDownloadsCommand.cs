using PalmClient.Services;
using System.Windows.Input;

namespace PalmClient.Commands
{
    public class NavigateDownloadsCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateDownloadsCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            // Call NavigateDownloads when the command is executed
            _navigationService.NavigateDownloads();
        }
    }
}
