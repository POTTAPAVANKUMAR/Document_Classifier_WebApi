
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Service
{
    public class PredictionService : IPredictionService
    {
        public async Task<List<PredictionResponse>> PredictAsync(List<string> texts)
        {
            // Perform prediction here
            return new List<PredictionResponse>
        {
            new PredictionResponse { Label = "Label1", Score = 0.9f },
            new PredictionResponse { Label = "Label2", Score = 0.8f }
        };
        }
    }
}
