using Document_Classifier_WebApi;
using Document_Classifier_WebApi.Service.Interface;
using System.Net.Http.Headers;

public class OcrService : IOcrService
{
    private readonly HttpClient _httpClient;

    public OcrService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> PerformOcrAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is not provided or is empty", nameof(file));
        }

        using (var content = new MultipartFormDataContent())
        {
            using (var fileStream = file.OpenReadStream())
            {
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.FileName);

                var response = await _httpClient.PostAsync($"{AppConst.DocumentClassifierServiceUrl}/ocr", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }
    }
}
