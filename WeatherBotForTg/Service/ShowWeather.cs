using Azure.AI.OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WeatherBotForTg.Service
{
    public class ShowWeather
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private static async Task<string> GetWeather(double latitude, double longitude)
        {
            string lat = Convert.ToString(latitude);
            string lon = Convert.ToString(longitude);
            string jsonString;

            string url = @$"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid=0b98b636af8a3c4cbf43994442587f0e";

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Обработка ошибки, например, выброс исключения или логирование
                    jsonString = string.Empty;
                }
            }
            return jsonString;
        }

        public static async Task<string> GetResultFromGpt(long id)
        {
            var locationTask = CheckService.CheckUserLocation(id);

            try
            {
                var location = await locationTask;

                if (location != null)
                {
                    var (latitude, longitude) = location.Value;
                    string informationWeather = await GetWeather(latitude, longitude);
                    string request = "расскажи про погоду вот так используя json выше, без лишних слов\r\nГород: Toshkent Shahri\r\n☁️ Погода: туман\r\n Температура: -17°C\r\n Ощущается как: -17°C\r\n‍ Видимость: 3км\r\n Скорость ветра: примерно 1.03 м/с, с юга\r\n Влажность: 85%\r\n Давление: 1034 гПа\r\n Время восхода солнца: 5:33\r\n Время заката солнца: 15:13\r\n Я рекомендую: Если вы собираетесь выходить на улицу, рекомендуется быть осторожными из-за тумана и надеть достаточно теплую одежду.";
                    string result = string.Empty;
                    string apiKeyForGpt = System.IO.File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\GPTApiKeyForWeather.txt");
                    OpenAIClient clientAI = new OpenAIClient(apiKeyForGpt);
                    var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
                    {
                        Messages = { new ChatMessage(ChatRole.System, informationWeather + "\n" + request) }
                    });
                    foreach (var item in openAIResponse.Value.Choices)
                    {
                        result += item.Message.Content;
                    }
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Обработка возможных исключений при вызове CheckUserLocation
                return null; // Или другой подходящий ответ в случае ошибки
            }
        }


    }
}
