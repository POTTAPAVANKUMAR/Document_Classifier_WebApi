using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Manager
{
    public class PredictManager : IPredictManager
    {
        private readonly IPredictionService _predictionService;

        public PredictManager(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public async Task<List<PredictionResponse>> PredictAsync(List<string> texts)
        {
            return await _predictionService.PredictAsync(texts);
        }
    }
}
