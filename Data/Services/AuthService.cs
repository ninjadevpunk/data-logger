using Core.Interfaces;
using Core.Models;
using Data.Interfaces;
using FileSys.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly IServiceProvider _serviceProvider;

        public ACCOUNT? Account { get; set; }


        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Result> SignUpAsync(string dp, string email, string password, string displayName, string surname, bool isEmployee,
    string companyName, string companyAddress, string companyLogo)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            string currentProfilePicPath = string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length <= 5)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return new Result { Success = false };

                if (string.IsNullOrWhiteSpace(password))
                    return new Result { Success = false };

                if (password.Length <= 5)
                {
                    return new Result
                    {
                        Success = false,
                        PublicMessage = "Your password is too short! Please enter a password that is at least 6 characters long.",
                        PublicMessageType = Result.MessageType.Error
                    };
                }
            }

            var cacheMaster = _serviceProvider.GetRequiredService<ICacheService>();

            try
            {
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                currentProfilePicPath = dp.Contains("/Assets") ? string.Empty : cacheMaster.SaveProfilePicture(dp);
                cacheMaster.DeleteTempProfilePics();

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

                bool userIsActive = await dataService.AddAccount(account);

                if (userIsActive)
                {
                    Account = account;
                    bool currentUserSet = await dataService.SetCurrentUser(account);

                    if (string.IsNullOrEmpty(currentProfilePicPath) && !dp.Contains("/Assets"))
                    {
                        return new Result
                        {
                            Success = currentUserSet,
                            PublicMessage = "We couldn't save your profile picture due to a technical issue. Your account has been created and a " +
                                                "default profile picture will be used instead.",
                            PublicMessageType = Result.MessageType.Warning
                        };
                    }

                    return new Result { Success = currentUserSet };
                }

                throw new InvalidOperationException("User is not active");
            }
            catch (Exception ex)
            {
                try
                {
                    var dataService = _serviceProvider.GetRequiredService<IDataService>();

                    if (!string.IsNullOrEmpty(currentProfilePicPath) && File.Exists(currentProfilePicPath))
                    {
                        try
                        {
                            File.Delete(currentProfilePicPath);
                        }
                        catch (Exception delex)
                        {
                            await dataService.HandleExceptionAsync(delex);
                        }
                    }

                    cacheMaster.DeleteTempProfilePics();

                    await dataService.HandleExceptionAsync(ex);
                }
                catch (Exception catchEx)
                {
                    Debug.WriteLine($"An error occurred in the catch block of SignUpAsync: {catchEx.Message}. This exception hasn't been logged to " +
                        $"the database.");
                }
            }

            return new Result
            {
                PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be " +
                        "sent to us.",
                PublicMessageType = Result.MessageType.Error
            };
        }

        public async Task<Result> SignInAsync(string email, string password)
        {
            try
            {
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    var (userFound, temporaryAccount) = await dataService.SignInUser(email, password);

                    if (!userFound)
                        return new Result();


                    Account = temporaryAccount;

                    return new Result
                    {
                        Success = userFound
                    };
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in SignInAsync: {ex.Message}. This exception hasn't been logged to the database.");
            }

            return new Result
            {
                PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                PublicMessageType = Result.MessageType.Error
            };

        }

        public async Task<Result> ForgotPasswordRequestAsync(string email)
        {
            // Validate email
            // Find account
            // Generate reset token/code
            // Send reset email
            // Store token/expiry
            // Return result

            try
            {

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in ForgotPasswordRequestAsync: {ex.Message}. This exception hasn't been logged to the database.");
            }

            throw new NotImplementedException();
        }

        public async Task<Result> ChangePasswordAsync(string newPassword, string email)
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();
                bool passwordChanged = false;


                try
                {
                    passwordChanged = await dataService.UpdateUserPasswordAsync(newPassword, email);

                    if (passwordChanged)
                    {
                        return new Result
                        {
                            Success = passwordChanged,
                            PublicMessage = "Your password has been changed successfully.",
                            PublicMessageType = Result.MessageType.Info,
                        };
                    }

                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in ChangePasswordAsync: {ex.Message}. This exception hasn't been logged to the database.");
            }

            return new Result
            {
                PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused. Feedback will automatically be sent to us.",
                PublicMessageType = Result.MessageType.Error
            };

        }

        public async Task<Result> DeleteAccountAsync()
        {
            try
            {
                bool accountDeleted = false;
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                if (Account != null)
                {
                    try
                    {
                        accountDeleted = await dataService.DeleteAccountByID(Account.accountID);

                        if (accountDeleted)
                            Account = null;
                    }
                    catch (Exception ex)
                    {
                        await dataService.HandleExceptionAsync(ex);

                        return new Result
                        {
                            Success = accountDeleted,
                            PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused. " +
                            "Feedback will automatically be sent to us.",
                            PublicMessageType = Result.MessageType.Error
                        };
                    }
                }

                return new Result
                {
                    Success = accountDeleted && Account == null,
                    PublicMessage = "Account deleted",
                    PublicMessageType = Result.MessageType.Info
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in DeleteAccountAsync: {ex.Message}. This exception hasn't been logged to the database.");

            }

            return new Result
            {
                PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused.",
                PublicMessageType = Result.MessageType.Error
            };
        }

        public async Task<Result> SignOutAsync()
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                try
                {
                    var signedOut = await dataService.SignOutUser();

                    if (signedOut)
                        Account = null;

                    return new Result
                    {
                        Success = Account == null && signedOut
                    };
                }
                catch (Exception ex)
                {
                    await dataService.HandleExceptionAsync(ex);

                    return new Result
                    {
                        PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused. " +
                        "Feedback will automatically be sent to us.",
                        PublicMessageType = Result.MessageType.Error
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred in SignOutAsync: {ex.Message}. This exception hasn't been logged to the database.");

                return new Result
                {
                    PublicMessage = "A problem occurred on our end. We apologise for any inconvenience caused.",
                    PublicMessageType = Result.MessageType.Error
                };
            }
        }
    }
}
