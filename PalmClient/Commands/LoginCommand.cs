using PalmClient.Services;
using PalmClient.ViewModels;
using System;

namespace PalmClient.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        private readonly LoginViewModel _viewModel;

        public LoginCommand(INavigationService navigationService, LoginViewModel viewModel)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        }

        public override void Execute(object? parameter)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(_viewModel.Username) || string.IsNullOrEmpty(_viewModel.Password))
            {
                _viewModel.ErrorMessage = "Please enter both username and password.";
                return;
            }

            // Hardcoded authentication
            if (_viewModel.Username == "admin" && _viewModel.Password == "12345")
            {
                // Navigate to downloads on successful login
                _navigationService.NavigateDownloads();
            }
            else
            {
                // Update the error message in ViewModel
                _viewModel.ErrorMessage = "Invalid username or password.";
            }
        }
    }
}
