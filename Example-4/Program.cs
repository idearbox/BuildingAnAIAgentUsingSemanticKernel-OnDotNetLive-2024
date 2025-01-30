using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

string[] systemMessages = ["너는 회사에 다니는 무뚝뚝한 아저씨처럼 대답해야해, 매번에 아재 개그를 추가해줘, 매번 응답 후에 너의 직업을 말해야 해",
                           "너는 한국 사회에가 가장 두려움이 없는 존재인 아줌마야, 아춤마 처럼 대답해야해, 매번 응답 후에 너의 직업을 말해야 해",
                           "너는 사춘기의 중학생 남자야 한국의 중학생처럼 대답해야해, 매번 응답 후에 너의 직업을 말해야 해",
                           "너는 초등학생 남자야 초등학생 남자처럼 대답해야해, 매번 응답 후에 너의 직업을 말해야 해",
                           "너는 초등학생 여자야 초등학생 여자처럼 대답해야해, 매번 응답 후에 너의 직업을 말해야 해",
                           "너는 유지원생 여자야 유치원생 여자처럼 대답해야해, 매번 응답 후에 너의 직업을 말해야 해",
                          ];

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
int index = 0;

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
    if(index >= systemMessages.Length)
    {
        index = 0;
    }
    history.AddSystemMessage(systemMessages[index++]);
    //history.AddSystemMessage($"너는 {age}살 아이처럼 대답하고, 매번 응답 후에 너의 나이를 말해야 해.");
}