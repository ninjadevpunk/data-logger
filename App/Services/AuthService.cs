using Data_Logger_1._3.Models;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Data_Logger_1._3.Services
{
    public class AuthService
    {
        private readonly IServiceProvider _serviceProvider;

        public ACCOUNT? Account { get; set; }


        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> SignUp(string dp, string email, string password, string displayName, string surname, bool isEmployee, 
            string companyName, string companyAddress, string companyLogo)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            string currentProfilePicPath = string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length <= 5)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                if (string.IsNullOrWhiteSpace(password))
                    return false;

                if (password.Length <= 5)
                {
                    MessageBox.Show("Your password is too short! Please enter a password that is at least 6 characters long.", "Password Too Short", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }

            try
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();
                currentProfilePicPath = dp.Contains("/Assets") ? string.Empty : BitmapService.SaveProfilePicture(dp);
                BitmapService.DeleteTempProfilePics();

                var account = new ACCOUNT
                {
                    ProfilePic = currentProfilePicPath,
                    FirstName = displayName,
                    LastName = surname,
                    Email = email,
                    Password = password,
                    IsEmployee = isEmployee,
                    CompanyName = companyName,
                    CompanyAddress = companyAddress,
                    CompanyLogo = companyLogo,
                    IsOnline = true,
                };

                bool UserIsActive = await writer.AddAccount(account);

                if (UserIsActive)
                {
                    Account = account;

                    if (string.IsNullOrEmpty(currentProfilePicPath) && !dp.Contains("/Assets"))
                    {
                        MessageBox.Show("We couldn't save your profile picture due to a technical issue. Your account has been created and a default profile picture will be used instead.",
                            "Profile Picture", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    return await writer.SetCurrentUser(account);
                }

            }
            catch (Exception ex)
            {
                var writer = scope.ServiceProvider.GetRequiredService<EntityWriter>();

                if (!string.IsNullOrEmpty(currentProfilePicPath) && File.Exists(currentProfilePicPath))
                {
                    try
                    {
                        File.Delete(currentProfilePicPath);
                    }
                    catch (Exception delex)
                    {
                        await writer.HandleExceptionAsync(delex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");
                    }
                }
                BitmapService.DeleteTempProfilePics();

                await writer.HandleExceptionAsync(ex, "SignUp(dp,email,password,displayName,surname,isEmployee,companyName,companyAddress,companyLogo)");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            return false;

        }

        public async Task<bool> SignIn(string email, string password)
        {
            var dataService = _serviceProvider.GetRequiredService<IDataService>();

            try
            {


                var (userFound, temporaryAccount) = await dataService.SignInUser(email, password);
                if (!userFound)
                    return false;


                Account = temporaryAccount;

                return userFound;
            }
            catch (Exception ex)
            {

                await dataService.HandleExceptionAsync(ex, "SignIn(email, password)");

                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            return false;
        }

        public void ForgotPasswordRequest()
        {
            //
        }

        public async Task<bool> ChangePassword(string newPassword, string email)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
            bool passwordChanged = false;


            try
            {
                passwordChanged = await dataService.UpdateUserPasswordAsync(newPassword, email);

                if(passwordChanged)
                {
                    MessageBox.Show("Your password has been changed successfully.", "Password Changed", MessageBoxButton.OK,
                    MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
            catch (Exception ex)
            {
                await dataService.HandleExceptionAsync(ex, "ChangePassword(email, oldPassword, newPassword)");
                MessageBox.Show("A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                    "Error Occurred", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }

            return passwordChanged;
        }

        public async Task<bool> DeleteAccountAsync()
        {
            bool accountDeleted = false;

            if (Account != null)
            {
                var handler = _serviceProvider.GetRequiredService<EntityHandler>();
                accountDeleted = await handler.DeleteAccountAsync(Account.accountID);
                Account = null;
            }

            return accountDeleted;
        }

        public void SignOut()
        {
            Account = null;
        }
    }
}
