namespace Document_Classifier_WebApi.Manager.Interface
{
    public interface IOcrManager
    {
        Task<string> PerformOcrAsync(IFormFile file);
    }
}
