using Document_Classifier_WebApi.Manager.Interface;
using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Manager
{
    public class TrainManager : ITrainManager
    {
        private readonly ITrainingService _trainingService;

        public TrainManager(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        public async Task<string> TrainAsync(List<TrainingData> trainingData)
        {
            return await _trainingService.TrainAsync(trainingData);
        }
    }
}
