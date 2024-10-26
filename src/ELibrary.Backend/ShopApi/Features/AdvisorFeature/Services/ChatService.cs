using LangChain.Databases.Postgres;
using LangChain.DocumentLoaders;
using LangChain.Extensions;
using LangChain.Providers;
using LangChain.Providers.OpenAI;
using LangChain.Providers.OpenAI.Predefined;
using LibraryShopEntities.Data;
using LibraryShopEntities.Domain.Entities.Library;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using System.Text;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatConfiguration chatConfig;
        private readonly PostgresVectorDatabase vectorDatabase;
        private readonly IDatabaseRepository<LibraryShopDbContext> repository;
        private readonly OpenAiLatestFastChatModel llm;
        private readonly OpenAiProvider provider;

        public ChatService(IDatabaseRepository<LibraryShopDbContext> repository, IConfiguration configuration)
        {
            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                      .Get<ChatConfiguration>()!;

            this.repository = repository;

            provider = new OpenAiProvider(chatConfig.OpenAiApiKey);
            llm = new OpenAiLatestFastChatModel(provider);

            vectorDatabase = new PostgresVectorDatabase(chatConfig.DbConnectionString);
        }

        public async Task<StringBuilder> AskQuestionAsync(string question, List<Document> documents, CancellationToken cancellationToken)
        {
            var embeddingModel = new TextEmbeddingV3SmallModel(provider);
            var vectorCollection = await vectorDatabase.GetOrCreateCollectionAsync(chatConfig.CollectionName, chatConfig.CollectionDimensions, cancellationToken);

            if (documents.Count > 0)
            {
                await vectorCollection.AddDocumentsAsync(embeddingModel, documents, cancellationToken: cancellationToken);
            }

            var similarDocuments = await vectorCollection.GetSimilarDocuments(
                embeddingModel: embeddingModel,
                request: new EmbeddingRequest() { Strings = new List<string> { question } },
                scoreThreshold: chatConfig.ScoreThreshold,
                amount: chatConfig.MaxAmountOfSimmilarDocuments,
                cancellationToken: cancellationToken
            );

            string request = chatConfig.GroundedPrompt.Replace("{sources}", string.Join("\n", similarDocuments)).Replace("{question}", question);

            var responseEnumerator = llm.GenerateAsync(request, cancellationToken: cancellationToken);

            StringBuilder result = new StringBuilder();
            await foreach (var chunk in responseEnumerator)
            {
                result.Append(chunk.LastMessageContent);
            }

            return result;
        }
        public async Task<List<Document>> GetDocumentsAsync(CancellationToken cancellationToken)
        {
            var queryable = await repository.GetQueryableAsync<Book>(cancellationToken);

            var bookDetails = await queryable
                .AsNoTracking()
                .AsSplitQuery()
                .Select(book => new
                {
                    BookName = book.Name,
                    AuthorName = book.Author.Name,
                    GenreName = book.Genre.Name,
                    PublisherName = book.Publisher == null ? "NONE" : book.Publisher.Name,
                    BookId = book.Id
                })
                .ToListAsync(cancellationToken);

            var documents = bookDetails.Select(detail => new Document(
                content: $"Book: {detail.BookName}, Author: {detail.AuthorName}, Genre: {detail.GenreName}, Publisher: {detail.PublisherName}",
                metadata: new Dictionary<string, object>
                {
                    { "bookId", detail.BookId },
                    { "bookName", detail.BookName },
                    { "authorName", detail.AuthorName },
                    { "genreName", detail.GenreName },
                    { "publisherName", detail.PublisherName }
                }
            )).ToList();

            return documents;
        }
    }
}
