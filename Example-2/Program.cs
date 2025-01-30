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

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

while (true)
{
    Console.WriteLine(  ); 
    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("User > ");
    Console.ForegroundColor = ConsoleColor.White;
    var request = Console.ReadLine();

    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(request!, kernel: kernel);

    string fullMessage = "";
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("Assistant > ");

    await foreach (var content in result)
    {
        Console.Write(content.Content);
        fullMessage += content.Content;
    }

    Console.WriteLine();
}