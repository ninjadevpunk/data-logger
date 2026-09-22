using Data_Logger_1._3.Models.DTOs;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Data_Logger_1._3.Services.CommandLogic
{
    public class PasswordResetService
    {
        private readonly IDataService _dataService;

        public PasswordResetService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<bool> RequestPasswordResetAsync(string email)
        {
            try
            {
                // Check local DB first.
                if (await _dataService.EmailExists(email))
                {
                    using var client = new HttpClient();

                    var payload = new
                    {
                        action = "code-request",
                        email
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://datalogger.space/api/verification", content);

                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException httpEx)
            {
                await _dataService.HandleExceptionAsync(httpEx, "RequestPasswordResetAsync()");
                return false;
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "RequestPasswordResetAsync()");
                return false;
            }

            return true;
        }

        public async Task<bool> VerifyCodeAsync(string email, string? code)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    using var client = new HttpClient();

                    var payload = new
                    {
                        action = "verify",
                        email,
                        code
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("https://datalogger.space/api/verification", content);

                    response.EnsureSuccessStatusCode();

                    // Deserialize the response to check if the verification was successful
                    var responseBody = await response.Content.ReadAsStringAsync();

                    var result = JsonConvert.DeserializeObject<VerificationResponseDTO>(responseBody);

                    return result?.Verified ?? false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                await _dataService.HandleExceptionAsync(httpEx, "VerifyCodeAsync()");
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex, "VerifyCodeAsync()");
            }

            return false;
        }
    }
}
