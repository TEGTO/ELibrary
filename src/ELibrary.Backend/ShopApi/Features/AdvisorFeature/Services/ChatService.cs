using OpenAI.Chat;

namespace ShopApi.Features.AdvisorFeature.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatConfiguration chatConfig;

        public ChatService(IConfiguration configuration)
        {
            chatConfig = configuration.GetSection(Configuration.CHAT_CONFIGURATION_SECTION)
                                          .Get<ChatConfiguration>()!;
        }

        public async Task<string> GetChatCompletionAsync(string prompt, string query, CancellationToken cancellationToken)
        {
            var chatClient = CreateChatClient();
            var chatCompletion = await chatClient.CompleteChatAsync(
                new List<ChatMessage>
                {
                new SystemChatMessage(prompt),
                new UserChatMessage(query)
                },
                cancellationToken: cancellationToken
            );

            return chatCompletion.Value.Content.FirstOrDefault()?.Text ?? "";
        }
        private ChatClient CreateChatClient()
        {
            return new ChatClient(chatConfig.ChatModel, apiKey: chatConfig.OpenAiApiKey);
        }
    }
}
