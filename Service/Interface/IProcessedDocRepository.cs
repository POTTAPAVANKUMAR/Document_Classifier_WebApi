namespace Document_Classifier_WebApi.Service.Interface
{
    public interface IProcessedDocRepository
{
    Task SaveAsync(ProcessedDocument document);
    Task<ProcessedDocument> GetByFileNameAsync(string fileName);
}
}