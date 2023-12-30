using Azure.AI.OpenAI;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WeatherBotForTg.Service
{
    public class ShowWeather
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static string apiKeyForWeather = System.IO.File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\OpenWeatherMapApi.txt");
        private static string apiKeyForGpt = System.IO.File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\GPTApiKeyForWeather.txt");
        public static async Task ShowWeatherGpt(Update update, ITelegramBotClient botClient)
        {
            var user = update.Message.Chat;
            var resulet = await GetResultFromGpt(user.Id);
            if (resulet != null)
                await botClient.SendTextMessageAsync(user.Id, resulet);
            else
                await botClient.SendTextMessageAsync(user.Id, "Ваши кординаты не найдены, устоновите их!");
        }

        private static async Task<string> GetWeather(double latitude, double longitude)
        {
            string lat = Convert.ToString(latitude);
            string lon = Convert.ToString(longitude);
            string jsonString;

            string url = @$"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKeyForWeather}";

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                    jsonString = await response.Content.ReadAsStringAsync();
                else
                {
                    throw new Exceptions.ExceptionHTTPResponse("ошибка при получений о информаций о погоде | проверте внутри " +
                        "Service класс ShowWeather метод GetWeather(double latitude, double longitude)");
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
                    JObject jsonData = JObject.Parse(informationWeather);
                    string description = (string)jsonData["weather"][0]["description"];
                    double temp = (double)jsonData["main"]["temp"];
                    double feelsLike = (double)jsonData["main"]["feels_like"];
                    int humidity = (int)jsonData["main"]["humidity"];
                    double windSpeed = (double)jsonData["wind"]["speed"];
                    string cityName = (string)jsonData["name"];
                    long visibility = (long)jsonData["visibility"];
                    string request = $"Город: {cityName}\r\n Погода: {description}\r\n Температура: {temp-273,15}°C\r\n Ощущается как: {feelsLike-273,15}°C\r\n‍ Видимость: {visibility/1000}km\r\n Скорость ветра: {windSpeed} м/с" +
                        $"\r\n Влажность: {humidity}\r\n Поставь подходящие сайлики перед каждым пунктом. Дай короткую рекомендацию по погоде, без ссылок исходя только из выше указанных данных. Как показано ниже\n";
                    string example = "📍Город: \r\n☁️ Погода: \r\n🌡️ Температура: °C\r\n🌡️ Ощущается как: °C\r\n‍🌫️ Видимость:  км\r\n💨 Скорость ветра:  м/с\r\n💦 Влажность: %\r\n" +
                        "Рекомендация: Сегодня ясная погода, температура небольшая, но ощущается еще прохладнее из-за ветра. Рекомендуется надеть теплую одежду и обувь, чтобы быть защищенным от холода..";
                    string result = string.Empty;
                    OpenAIClient clientAI = new OpenAIClient(apiKeyForGpt);
                    var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
                    {
                        Messages = { new ChatMessage(ChatRole.System, request+ example) }
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
                throw new Exception(ex.Message, ex);
                // Обработка возможных исключений при вызове CheckUserLocation
                return null; // Или другой подходящий ответ в случае ошибки
            }
        }


    }
}
