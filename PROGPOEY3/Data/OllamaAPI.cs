using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PROGPOEY3.Data
{
    public class OllamaAPI
    {
        private const string url = "http://localhost:11434";
        private const string postEndpoint = "generate";
        public string output;

        public event Action<string, bool>? OnStringProcessed;

        public async Task<string> MakePostRequest(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set up the URL and requestMessage body
                string url = "http://localhost:11434/api/generate";
                var requestBody = new
                {
                    model = "llama3",
                    prompt = request,
                    stream = true // Enable streaming
                };

                // Serialize the requestMessage body to JSON
                string json = JObject.FromObject(requestBody).ToString();

                // Create the StringContent for the requestMessage
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                // Create an HTTP requestMessage message
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                // Send the POST requestMessage
                HttpResponseMessage response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

                // Ensure the response is successful
                response.EnsureSuccessStatusCode();

                // Read and process the stream
                using (StreamReader reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    string line;

                    Console.ForegroundColor = ConsoleColor.Cyan;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                // Deserialize the JSON object
                                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(line);

                                // Add the deserialized object to the list
                                output += apiResponse.response;

                               

                                // Trigger the event with the processed string
                                OnStringProcessed?.Invoke(apiResponse.response!, apiResponse.done);
                            } catch (Exception ex)
                            {
                                Console.WriteLine($"Error parsing JSON: {ex.Message}");
                            }
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("");
                    return "\nSuccess";
                }
            }
        }
    }

    public struct ApiResponse 
    {
        private string model { get; set; }
        private string createdAt { get; set; }
        public string? response { get; set; }
        public bool done { get; set; }
        private int? totalDuration { get; set; }
        private int? loadDuration { get; set; }
        private int? promptEvalCount { get; set; }
        private int? promptEvalDuration { get; set; }
        private int? evalCount { get; set; }
        private int? evalDuration { get; set; }
    }
}
