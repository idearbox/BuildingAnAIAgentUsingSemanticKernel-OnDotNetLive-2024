using Microsoft.SemanticKernel;


var model = Environment.GetEnvironmentVariable("AZURE_OPENAI_MODEL_NAME1");
var endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT1");
var key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY1");

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
         model,   // Azure OpenAI Deployment Name
         endpoint,  // Azure OpenAI Endpoint
         key);      // Azure OpenAI Key
var kernel = builder.Build();

// Semantic Kernel를 활용한 prompt 실행
//string request = "LOL(롤) 게임의 챔피언 Mel(멜)에 대해서 아는 정보 있는가?";
//string request = "LOL(롤) 게임의 챔피언 나서스에 대해서 아는 정보 있는가? 간략히 3줄로 요약해줘";
string request = "LOL(롤) 게임의 챔피언 정보를 알고싶어";
string prompt = $"이 요청의 의도는 무엇입니까? {request}";
Console.WriteLine($"1..----------------------------------------------------------------------"); 
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();


// 프롬프트 엔지니어링으로 프롬프트 개선
prompt = @$"이 요청의 의도는 무엇입니까? {request}
            다음 중에서 선택할 수 있습니다: GetChamInfo, GetTodayCham, GetCurrentUserCount, GetServiceStatus.";
Console.WriteLine($"2..----------------------------------------------------------------------");
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();


// output 에 특정 formatting 적용
prompt = @$"Instructions: 이 요청의 의도는 무엇입니까?
            가능한 답변  : GetChamInfo, GetTodayCham, GetCurrentUserCount, GetServiceStatus
            User Input  : {request}
            Intent      : ";
Console.WriteLine($"3..----------------------------------------------------------------------");
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();


prompt = $$"""
         ## Instructions
         요청의 의도를 다음 형식으로 제공하십시오:

         ```json
         {
             "intent": {intent}
         }
         ```

         ## Choices
         다음 중에서 선택할 수 있습니다:

         ```json
         ["GetChamInfo", "GetTodayCham", "GetCurrentUserCount", "GetServiceStatus"]
         ```

         ## User Input
         사용자 요청:

         ```json
         {
             "request": "{{request}}"
         }
         ```

         ## Intent
         """;
Console.WriteLine($"4..----------------------------------------------------------------------");
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();
Console.ReadLine();

// Provide examples with few-shot prompting
prompt = @$"지침          : 이 요청의 의도는 무엇입니까?
            선택지        : GetChamInfo, GetTodayCham, GetCurrentUserCount, GetServiceStatus
            
            사용자 입력   : 오늘의 추전 챔피언은 누구인가요?
            의도          : GetTodayCham
            
            사용자 입력   : 현재 서비스 상태를 알려주세요.
            의도          : GetServiceStatus

            사용자 입력   : {request}
            의도: ";
Console.WriteLine($"5..----------------------------------------------------------------------");
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();

// Tell the AI what to do to avoid doing something wrong
prompt = @$"지침          : 이 요청의 의도는 무엇입니까?
                            의도를 정확히 모르면 추측하지 말고 대신 ""Unknown""이라고 응답하세요.
            선택지        : GetChamInfo, GetTodayCham, GetCurrentUserCount, GetServiceStatus
            
            사용자 입력   : 오늘의 추전 챔피언은 누구인가요?
            의도          : GetTodayCham
            
            사용자 입력   : 현재 서비스 상태를 알려주세요.
            의도          : GetServiceStatus

            사용자 입력   : {request}
            의도: ";

Console.WriteLine($"6..----------------------------------------------------------------------");
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine();

Console.ReadKey();