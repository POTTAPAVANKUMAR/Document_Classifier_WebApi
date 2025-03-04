using Document_Classifier_WebApi.Model;

namespace Document_Classifier_WebApi.Manager.Interface
{
    public interface ITrainManager
    {
        Task<string> TrainAsync(List<TrainingData> trainingData);
    }
}
