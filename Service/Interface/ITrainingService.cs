using Document_Classifier_WebApi.Model;

namespace Document_Classifier_WebApi.Service.Interface
{
    public interface ITrainingService
    {
        Task<string> TrainAsync(List<TrainingData> trainingData);
    }
}
