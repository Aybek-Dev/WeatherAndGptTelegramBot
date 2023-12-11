using Azure.AI.OpenAI;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace WeatherBotForTg
{
    class Program
    {
        private const string url = "https://api.openweathermap.org/data/2.5/weather?lat=41.31&lon=69.28&appid=0b98b636af8a3c4cbf43994442587f0e";
        private const string request = "расскажи про погоду вот так без лишних слов\r\nГород:  Ташкент\r\nпогода:  дождь\r\nтемпература: -10°C\r\nощущается как: -15°C\r\nвидимость: 2000м\r\nскорость ветра: примерно 2.06 м/с, с северо-запада.\r\nвлажность: 90%\r\nДавление: 1021 гПа\r\nВремя восхода солнца: 7:00\r\nВремя заката солнца: 19:00\r\nя рекомендую: Если вы собираетесь выходить на улицу, рекомендуется надеть теплую одежду, так как довольно прохладно, и возможен туман.\r\nи добавь подходящий смайлики";
        static string finaliResult = String.Empty;
        static private string tokek { get; set; } = "6679592489:AAFW3B8mwdZo17GUh0r2kHTG5Ta8KnGOoGY";
        private static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            try
            {
                string jsonString;
                using (HttpClient client = new HttpClient())
                {
                    jsonString = await client.GetStringAsync(url);
                    await Console.Out.WriteLineAsync(jsonString);
                }
                jsonString += request;
                OpenAIClient clientAI = new OpenAIClient("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");
                var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions 
                {
                    Messages = { new ChatMessage(ChatRole.System,jsonString)}
                });
                //botClient = new TelegramBotClient(tokek);
                //botClient.OnMessage += OnMessageHandler;

                finaliResult = String.Empty;
                foreach (var item in openAIResponse.Value.Choices)
                {
                    finaliResult += item.Message.Content;
                    //sawait Console.Out.WriteLineAsync(item.Text);
                }
                await Console.Out.WriteLineAsync(finaliResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text)
                return;

            // Обработка входящего текстового сообщения
            Console.WriteLine($"Получено текстовое сообщение: {message.Text}");
            await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text:$"{finaliResult}" ).ConfigureAwait(false);

            // Добавьте свою логику обработки сообщения или другие действия.
        }

    }
}