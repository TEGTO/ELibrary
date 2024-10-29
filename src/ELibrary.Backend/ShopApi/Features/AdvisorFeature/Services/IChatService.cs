
using LangChain.DocumentLoaders;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public interface IChatService
    {
        public Task<StringBuilder> AskQuestionAsync(string question, List<Document> documents, CancellationToken cancellationToken);
        public Task<List<Document>> GetDocumentsAsync(CancellationToken cancellationToken);
    }
}