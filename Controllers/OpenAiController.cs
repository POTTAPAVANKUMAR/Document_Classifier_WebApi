using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Document_Classifier_WebApi.Controllers
{
    [ApiController]
    [Route("openai")]
    public class OpenAiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OpenAiController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("analyze-document")]
        public async Task<IActionResult> AnalyzeDocument([FromBody] OpenAiDocRequest request)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return BadRequest("OpenAI API key is missing.");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

string fullPrompt = @$" You are an intelligent document assistant. Analyze the following document text and respond in this JSON format: {{ ""documentType"": ""<type of document>"", ""entities"": [""<entity1>"", ""<entity2>"", ...], ""verificationSteps"": [""<step1>"", ""<step2>"", ...] }} Document Text: {request.DocumentText} ";

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = fullPrompt }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(responseBody);
            var resultText = doc.RootElement
                                .GetProperty("choices")[0]
                                .GetProperty("message")
                                .GetProperty("content")
                                .GetString();

            // Parse the returned JSON string from GPT if it's valid JSON
            try
            {
                var structuredResponse = JsonDocument.Parse(resultText);
                return Ok(structuredResponse.RootElement);
            }
            catch
            {
                return Ok(new { raw = resultText });
            }
        }
    }

    public class OpenAiDocRequest
    {
        public string DocumentText { get; set; }
    }
}
