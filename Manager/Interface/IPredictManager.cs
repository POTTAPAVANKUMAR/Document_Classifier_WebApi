using Document_Classifier_WebApi.Model;

namespace Document_Classifier_WebApi.Manager.Interface
{
    public interface IPredictManager
    {
        Task<List<PredictionResponse>> PredictAsync(List<string> texts);
    }
}
