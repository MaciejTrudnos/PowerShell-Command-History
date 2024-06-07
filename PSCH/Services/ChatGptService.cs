using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace PSCH.Services
{
    public class ChatGptService
    {
        private readonly OpenAIService _openAiService;
        private readonly string _model;

        public ChatGptService(string apiKey, string model)
        {
            _openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = apiKey
            });

            _model = model;
        }

        public async Task<string> SendMessage(List<ChatMessage> messages)
        {
            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = messages,
                Model = _model,
            });

            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            };

            throw new HttpRequestException(completionResult.Error.Message);
        }
    }
}
