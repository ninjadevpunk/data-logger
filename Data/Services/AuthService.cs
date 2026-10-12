using Core.Interfaces;
using Core.Models;
using Core.Models.App_Models;
using Data.Interfaces;
using FileSys.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Data.Services
{
    public class AuthService : IAuthService
    {
        private readonly IServiceProvider _serviceProvider;

        private UserSettings Account { get; set; } = null!;


        public AuthService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Retrieve the logged in user.
        /// </summary>
        /// <returns>User settings</returns>
        public UserSettings GetUser() => Account ?? throw new ArgumentNullException(nameof(Account), "GetUser() returned null!");

        /// <summary>
        /// Refreshes Account property if a user has been modified.
        /// </summary>
        /// <returns>True if user is updated; otherwise false</returns>
        public async Task<bool> UpdateUser()
        {
            var dataService = _serviceProvider.GetRequiredService<IDataService>();
            var account = await dataService.RetrieveUserAsync();

            if (account == null)
                return false;

            Account = new UserSettings
            {
                Id = account.UserId,
                ProfilePic = account.ProfilePic,
                Name = account.Name,
                Surname = account.Surname,
                Email = account.Email,
                IsCompanyEmployee = account.IsEmployee,
                CompanyName = account.CompanyName,
                CompanyAddress = account.CompanyAddress,
                CompanyLogo = account.CompanyLogo
            };

            return true;
        }

        /// <summary>
        /// Validates a user's email before processing
        /// </summary>
        /// <param name="email">User's raw email input</param>
        /// <returns></returns>
        private static bool IsValidEmail(string emailInput)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            return Regex.IsMatch(emailInput, pattern);
        }

        /// <summary>
        /// Signs up a user. Throws a <see cref="FormatException"/> if the user enter's an invalid email and an
        /// <see cref="InvalidOperationException"/> if the user isn't logged in successfully.
        /// </summary>
        /// <param name="dp">User's profile picture</param>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <param name="displayName">User's name</param>
        /// <param name="surname">User's surname</param>
        /// <param name="isEmployee">User's employment status</param>
        /// <param name="companyName">Company the user/employee works for</param>
        /// <param name="companyAddress">Employee's company address</param>
        /// <param name="companyLogo">Employee's company logo</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the user cannot be successfully logged in.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown if the provided email address is invalid.
        /// </exception>
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
                if (!IsValidEmail(email))
                    throw new FormatException("Email is invalid");

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

                (bool userIsActive, int userId) = await dataService.AddAccountAsync(account);

                if (userIsActive)
                {
                    Account = new UserSettings
                    {
                        Id = userId,
                        Name = displayName,
                        Surname = surname,
                        Email = email,
                        IsCompanyEmployee = isEmployee,
                        CompanyName = companyName,
                        CompanyAddress = companyAddress,
                        CompanyLogo = companyLogo,
                    };
                    bool currentUserSet = await dataService.SetCurrentUserAsync(userId);

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
            catch (FormatException formx)
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

                    await dataService.HandleExceptionAsync(formx);
                }
                catch (Exception catchEx)
                {
                    Debug.WriteLine($"An error occurred in the catch block of SignUpAsync: {catchEx.Message}. This exception hasn't been logged to " +
                        $"the database.");
                }

                return new Result
                {
                    PublicMessage = "Please enter a valid email address.",
                    PublicMessageType = Result.MessageType.Error
                };
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

        /// <summary>
        /// Signs in a user. Throws an <see cref="ArgumentNullException"/> if user is not logged in.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>A result containing whether operation succeeded or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when user's account isn't logged in</exception>
        public async Task<Result> SignInAsync(string email, string password)
        {
            try
            {
                var dataService = _serviceProvider.GetRequiredService<IDataService>();

                try
                {
                    var (userFound, userDTO) = await dataService.SignInUserAsync(email, password);


                    if (!userFound)
                        return new Result();

                    if (userDTO == null)
                        throw new ArgumentNullException(null, "Account is null!");

                    var temporaryAccount = new UserSettings
                    {
                        Id = userDTO.UserId,
                        Name = userDTO.Name,
                        Surname = userDTO.Surname,
                        Email = userDTO.Email,
                        ProfilePic = userDTO.ProfilePic,
                        IsCompanyEmployee = userDTO.IsEmployee,
                        CompanyName = userDTO.CompanyName,
                        CompanyAddress = userDTO.CompanyAddress,
                        CompanyLogo = userDTO.CompanyLogo
                    };


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

        // TODO
        /// <summary>
        /// Sends verification code to user's email.
        /// </summary>
        /// <param name="email">The user's email regardless of if it exists in the database or not</param>
        /// <returns>A result on whether the request succeeded or not.</returns>
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

        /// <summary>
        /// Changes user's password.
        /// </summary>
        /// <param name="newPassword">User's new password</param>
        /// <param name="email">User email</param>
        /// <returns>A result on whether it changed the password successfully or not.</returns>
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


        /// <summary>
        /// Delete's a user's account permanently.
        /// </summary>
        /// <returns>A result on whether or not the account was deleted successfully or not.</returns>
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
                        accountDeleted = await dataService.DeleteAccountByIDAsync(Account.Id);

                        if (accountDeleted)
                            Account = new();
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
                    Success = accountDeleted,
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

        /// <summary>
        /// Signs out a user.
        /// </summary>
        /// <returns>A result on whether or not the user was successfully signed out.</returns>
        public async Task<Result> SignOutAsync()
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var dataService = scope.ServiceProvider.GetRequiredService<IDataService>();

                try
                {
                    var signedOut = await dataService.SignOutUserAsync();

                    if (signedOut)
                        Account = new();

                    return new Result
                    {
                        Success = signedOut
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
