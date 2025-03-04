using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Service
{
    public class OcrService : IOcrService
    {
        public async Task<string> PerformOcrAsync(IFormFile file)
        {
            // Perform OCR processing here
            return "Extracted text from image"; // Placeholder
        }
    }
}
