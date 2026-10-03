using Core.Models;
using Core.Models.App_Models;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Retrieve the logged in user.
        /// </summary>
        /// <returns>User settings</returns>
        UserSettings GetUser();

        /// <summary>
        /// Signs up a user. Throws a <see cref="FormatException"/> if the user enters an invalid email and an
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
        Task<Result> SignUpAsync(string dp, string email, string password, string displayName, string surname, bool isEmployee, 
            string companyName, string companyAddress, string companyLogo);

        /// <summary>
        /// Signs in a user. Throws an <see cref="ArgumentNullException"/> if user is not logged in.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>A result containing whether operation succeeded or not</returns>
        /// <exception cref="ArgumentNullException">Thrown when user's account isn't logged in</exception>
        Task<Result> SignInAsync(string email, string password);

        /// <summary>
        /// Sends verification code to user's email.
        /// </summary>
        /// <param name="email">The user's email regardless of if it exists in the database or not</param>
        /// <returns>A result on whether the request succeeded or not.</returns>
        Task<Result> ForgotPasswordRequestAsync(string email);

        /// <summary>
        /// Changes user's password.
        /// </summary>
        /// <param name="newPassword">User's new password</param>
        /// <param name="email">User email</param>
        /// <returns>A result on whether it changed the password successfully or not.</returns>
        Task<Result> ChangePasswordAsync(string newPassword, string email);

        /// <summary>
        /// Delete's a user's account permanently.
        /// </summary>
        /// <returns>A result on whether or not the account was deleted successfully or not.</returns>
        Task<Result> DeleteAccountAsync();

        /// <summary>
        /// Signs out a user.
        /// </summary>
        /// <returns>A result on whether or not the user was successfully signed out.</returns>
        Task<Result> SignOutAsync();
    }
}
