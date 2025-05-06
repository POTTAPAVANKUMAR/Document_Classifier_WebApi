using System.Threading.Tasks;

namespace Document_Classifier_WebApi.Services
{
    public interface IOpenAiService
    {
        Task<object> AnalyzeDocumentAsync(string documentText);
    }
}
