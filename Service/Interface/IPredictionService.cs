using Document_Classifier_WebApi.Model;

namespace Document_Classifier_WebApi.Service.Interface
{
    public interface IPredictionService
    {
        Task<List<PredictionResponse>> PredictAsync(List<string> texts);
    }
}
