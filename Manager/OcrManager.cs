using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Manager
{
    public class OcrManager : IOcrManager
    {
        private readonly IOcrService _ocrService;

        public OcrManager(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        public async Task<string> PerformOcrAsync(IFormFile file)
        {
            return await _ocrService.PerformOcrAsync(file);
        }
    }
}
