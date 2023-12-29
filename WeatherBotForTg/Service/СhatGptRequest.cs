using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBotForTg.Service
{
    public class СhatGptRequest
    {
        public static async Task<string> ConnectFromGpt(string message)
        {
            string apiForChat = File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\GPTApiKeyForChat.txt");
            string finnalyAnsver = String.Empty;
            OpenAIClient clientAI = new OpenAIClient(apiForChat);
            var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
            {
                Messages = { new ChatMessage(ChatRole.System, message) }
            });
            foreach (var item in openAIResponse.Value.Choices)
            {
                finnalyAnsver += item.Message.Content;
            }
            return finnalyAnsver ?? "извените, но сервис не работает вресенно";
        }
    }
}
