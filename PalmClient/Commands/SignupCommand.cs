using PalmClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Commands
{
    class SignupCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public SignupCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.NavigateSignup();
        }
    }
}



