using Document_Classifier_WebApi.Model;
using Document_Classifier_WebApi.Service.Interface;

namespace Document_Classifier_WebApi.Service
{
    public class TrainingService : ITrainingService
    {
        public async Task<string> TrainAsync(List<TrainingData> trainingData)
        {
            // Perform training here
            return "Training completed successfully.";
        }
    }
}
