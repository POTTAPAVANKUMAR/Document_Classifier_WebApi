namespace Document_Classifier_WebApi.Service.Interface
{
    public interface IOcrService
    {
        Task<string> PerformOcrAsync(IFormFile file);
    }
}
