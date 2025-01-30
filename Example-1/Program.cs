using Microsoft.SemanticKernel;


var endpoint = "https://sample-lab-02.openai.azure.com";//Environment.GetEnvironmentVariable("OPENAI_ENDPOINT");
var key = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY1");

var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
         "gpt-4o",   // Azure OpenAI Deployment Name
         endpoint,  // Azure OpenAI Endpoint
         key);      // Azure OpenAI Key
var kernel = builder.Build();


// Running your first prompt with Semantic Kernel
string request = "내 태양광 패널이 얼마나 많은 전력을 제공하는지 알고 싶습니다.";
string prompt = $"이 요청의 의도는 무엇입니까? {request}";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("1..---------------------");
Console.WriteLine();

// Improving the prompt with prompt engineering
prompt = @$"이 요청의 의도는 무엇입니까? {request}
다음 중에서 선택할 수 있습니다: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("2..---------------------");
Console.WriteLine();

// Add structure to the output with formatting
prompt = @$"지침: 이 요청의 의도는 무엇입니까?
선택지: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.
사용자 입력: {request}
의도: ";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("3..---------------------");
Console.WriteLine();

prompt = $$"""
         ## 지침
         요청의 의도를 다음 형식으로 제공하십시오:

         
## 선택지
다음 의도 중에서 선택할 수 있습니다:
## 사용자 입력
사용자 입력은 다음과 같습니다:
## 의도
""";
prompt = $$"""
         ## Instructions
         Provide the intent of the request using the following format:

         ```json
         {
             "intent": {intent}
         }
         ```

         ## Choices
         You can choose between the following intents:

         ```json
         ["GetSolarEnergyToday", "GetSolarPower", "GetSolarBatteryPercentage", "StartChargingCar"]
         ```

         ## User Input
         The user input is:

         ```json
         {
             "request": "{{request}}"
         }
         ```

         ## Intent
         """;
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("4..---------------------");
Console.WriteLine();

// Provide examples with few-shot prompting
prompt = @$"지침: 이 요청의 의도는 무엇입니까?
선택지: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.

사용자 입력: 내 태양광 패널이 오늘 얼마나 많은 에너지를 제공했나요?
의도: GetSolarEnergyToday

사용자 입력: 내 차를 충전할 수 있나요?
의도: StartChargingCar

사용자 입력: {request}
의도: ";
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("5..---------------------");
Console.WriteLine();

// Tell the AI what to do to avoid doing something wrong
prompt = $"""
         지침: 이 요청의 의도는 무엇입니까?
         의도를 모르면 추측하지 말고 대신 "Unknown"이라고 응답하세요.
         선택지: GetSolarEnergyToday, GetSolarPower, GetSolarBatteryPercentage, StartChargingCar.

         사용자 입력: 내 태양광 패널이 오늘 얼마나 많은 에너지를 제공했나요?
         의도: GetSolarEnergyToday

         사용자 입력: 내 차를 충전할 수 있나요?
         의도: StartChargingCar

         사용자 입력: {request}
         의도: 
         """;
Console.WriteLine(await kernel.InvokePromptAsync(prompt));
Console.WriteLine("6..---------------------");
Console.WriteLine();

Console.ReadKey();