using PalmClient.Services;
using System;
using PalmClient.Commands;
using System.ComponentModel;
using System.IO;
using PalmClient.Core;
using System.Windows.Input;


namespace PalmClient.ViewModels
{
    public class SignupViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }

        private readonly INavigationService _navigationService;
        public ICommand LoginCommand { get; }
        public ICommand SignupCommand { get; }

        // Constructor
        public SignupViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // Initialize LoginCommand with the navigation service
           //LoginCommand = new LoginCommand(_navigationService);

            // Initialize LoginCommand with the navigation service
            SignupCommand = new SignupCommand(_navigationService);
        }

        // Method to handle signup functionality
        public void Signup()
        {
            // Check if all fields are filled
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Number) || string.IsNullOrEmpty(Password))
            {
                // Optionally show an error message (for UI validation)
                Console.WriteLine("Please fill all fields.");
                return;
            }

            // Save user data to file
            try
            {
                string userData = $"Name: {Name}\nEmail: {Email}\nNumber: {Number}\nPassword: {Password}\n\n";
                File.AppendAllText(@"C:\Users\HP\OneDrive\Desktop\User.txt", userData);
                Console.WriteLine("User data saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
                return;
            }

            // Navigate to LoginView
            _navigationService.NavigateSignup();
        }
    }
}
