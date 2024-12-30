using PalmClient.Enums;
using PalmClient.Events;
using PalmClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalmClient.Services
{
    public interface INavigationService
    {
        public event EventHandler<PageChangedEventArgs> PageChangedEvent;
        public PageType CurrentPage { get; }
        public void NavigateHome();
        public void NavigatePlaylist();
        public void NavigateDownloads();
        public void NavigateLogin();
        public void NavigateSignup();
    }

    public class NavigationService : INavigationService
    {
        private readonly Func<MainViewModel>? _mainViewModelFunc;
        private readonly Func<HomeViewModel>? _homeViewModelFunc;
        private readonly Func<PlaylistViewModel>? _playlistViewModelFunc;
        private readonly Func<DownloadsViewModel>? _downloadViewModelFunc;
        private readonly Func<LoginViewModel>? _loginViewModelFunc;
        private readonly Func<SignupViewModel>? _signupViewModelFunc;

        public event EventHandler<PageChangedEventArgs>? PageChangedEvent;
        public PageType CurrentPage { get; private set; } = PageType.Home;

        public NavigationService(Func<MainViewModel> mainViewModelFunc, Func<LoginViewModel> loginViewModelFunc, Func<SignupViewModel> signupViewModelFunc, Func<HomeViewModel> homeViewModelFunc,
                                 Func<PlaylistViewModel> playlistViewModelFunc, Func<DownloadsViewModel> downloadViewModelFunc)
        {
            _mainViewModelFunc = mainViewModelFunc;
            _loginViewModelFunc = loginViewModelFunc;
            _signupViewModelFunc = signupViewModelFunc;
            _homeViewModelFunc = homeViewModelFunc;
            _playlistViewModelFunc = playlistViewModelFunc;
            _downloadViewModelFunc = downloadViewModelFunc;
        }
        public void NavigateLogin()
        {
            // Ensure that the functions to retrieve the view models are not null
            var mainVm = _mainViewModelFunc?.Invoke();
            var loginVm = _loginViewModelFunc?.Invoke();

            // Check if both view models are valid (non-null)
            if (mainVm != null && loginVm != null)
            {
                // Set the current view to the LoginView model
                mainVm.CurrentView = loginVm;

                // Update the current page type to 'Login'
                CurrentPage = PageType.Login;

                // Fire the PageChangedEvent to notify that the page has changed
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
            else
            {
                // Optionally, handle the case where the view models are null
                // This could include logging an error, showing a message, or using a fallback view
                Debug.WriteLine("Error: Unable to navigate to the Login view. View models are not available.");
            }
        }

        public void NavigateSignup()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var signupVm = _signupViewModelFunc?.Invoke();

            if (mainVm != null && signupVm != null)
            {

                // Set the current view to the SignupViewModel
                mainVm.CurrentView = signupVm;

                // Update the current page to Signup
                CurrentPage = PageType.Signup;

                // Trigger the PageChangedEvent to notify the UI
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
        }


        public void NavigateHome()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var homeVm = _homeViewModelFunc?.Invoke();

            if (mainVm != null && mainVm.CurrentView is not HomeViewModel)
            {
                mainVm.CurrentView = homeVm;
                CurrentPage = PageType.Home;
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
        }

        public void NavigatePlaylist()
        {
            var mainVm = _mainViewModelFunc?.Invoke();
            var playlistVm = _playlistViewModelFunc?.Invoke();

            if (mainVm != null)
            {
                mainVm.CurrentView = playlistVm;
                CurrentPage = PageType.Playlist;
                PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
            }
        }

        public void NavigateDownloads()
        {
            // Get the current main view model and the downloads view model
            var mainVm = _mainViewModelFunc?.Invoke();
            var downloadsVm = _downloadViewModelFunc?.Invoke();

            // Check if the main view model and downloads view model are available
            if (mainVm != null && downloadsVm != null)
            {
                // Ensure that the current view is not already DownloadsViewModel
                if (mainVm.CurrentView is not DownloadsViewModel)
                {
                    // Set the current view to the DownloadsViewModel
                    mainVm.CurrentView = downloadsVm;

                    // Update the current page to Downloads
                    CurrentPage = PageType.Downloads;

                    // Trigger the PageChangedEvent to notify the UI
                    PageChangedEvent?.Invoke(this, new PageChangedEventArgs(CurrentPage));
                }
            }
        }

    }
}
