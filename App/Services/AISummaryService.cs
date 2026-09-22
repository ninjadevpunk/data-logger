using System.Net.Http;
using System.Text;

namespace Data_Logger_1._3.Services
{
    public class AISummaryService
    {
        private static readonly string apiKey = "GoogleAIKey";

        public static async Task<string> SummarizeText(string text)
        {
            using var client = new HttpClient();
            var requestBody = $"{{ \"contents\": [{{ \"parts\": [{{ \"text\": \"Summarize this: {text}\" }}] }}] }}";
            var response = await client.PostAsync(
                $"https://generativelanguage.googleapis.com/v1/models/gemini-pro:generateText?key={apiKey}",
                new StringContent(requestBody, Encoding.UTF8, "application/json")
            );

            return await response.Content.ReadAsStringAsync();
        }
    }
}
