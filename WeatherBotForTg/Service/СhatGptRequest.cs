using Azure.AI.OpenAI;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WeatherBotForTg.Service
{
    public class СhatGptRequest
    {
        public static async Task<string> ConnectFromGpt(string message)
        {
            string apiForChat = System.IO.File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\GPTApiKeyForChat.txt");
            string finnalyAnsver = String.Empty;

            await Task.Run(async () =>
            {
                OpenAIClient clientAI = new OpenAIClient(apiForChat);
                var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
                {
                    Messages = { new ChatMessage(ChatRole.System, message) }
                });

                foreach (var item in openAIResponse.Value.Choices)
                {
                    finnalyAnsver += item.Message.Content;
                }
            });

            return finnalyAnsver ?? "извините, но сервис не работает в данный момент";
        }

    }
}
