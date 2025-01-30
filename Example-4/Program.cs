using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;


var model = Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL_NAME1");
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT1");
var key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY1");

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
         model,   // Azure OpenAI Deployment Name
         endpoint,  // Azure OpenAI Endpoint
         key);      // Azure OpenAI Key
var kernel = builder.Build();

ChatHistory history = [];

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

history.AddSystemMessage("너는 10살 아이처럼 대답해야 해.");

int age = 10;

while (true)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();
    history.AddUserMessage(request!);

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(history, kernel: kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }

    Console.WriteLine();

    history.AddAssistantMessage(fullMessage);

    age -= 2;

    history.AddSystemMessage($"너는 {age}살 아이처럼 대답하고, 매번 응답 후에 너의 나이를 말해야 해.");
}