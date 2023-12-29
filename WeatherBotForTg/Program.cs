using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WeatherBotForTg
{
    class Program
    {
        private static ReceiverOptions _receiverOptions;
        static private string tokek { get; set; } = File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\BotToken.txt");
        private static ITelegramBotClient botClient;
        static async Task Main(string[] args)
        {
            //try
            //{
            //    string jsonString;
            //    using (HttpClient client = new HttpClient())
            //    {
            //        jsonString = await client.GetStringAsync(url);
            //        await Console.Out.WriteLineAsync(jsonString);
            //    }
            //    jsonString += request;
            //    OpenAIClient clientAI = new OpenAIClient("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");
            //    var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
            //    {
            //        Messages = { new ChatMessage(ChatRole.System, jsonString) }
            //    });

            //    finaliResult = String.Empty;
            //    foreach (var item in openAIResponse.Value.Choices)
            //    {
            //        finaliResult += item.Message.Content;
            //    }
            //    await Console.Out.WriteLineAsync(finaliResult);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Произошла ошибка: {ex.Message}");
            //

            //// botClient = new TelegramBotClient(tokek);
            ////// botClient.OnMessage += BotOnMessageReceived;
            //// botClient.StartReceiving();
            //// Console.ReadLine();
            //// botClient.StopReceiving();
            botClient = new TelegramBotClient(tokek); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
            _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
            {
                AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
                {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
                // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
                // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            // UpdateHander - обработчик приходящих Update`ов
            // ErrorHandler - обработчик ошибок, связанных с Bot API

            botClient.StartReceiving(ChatHendler.UpdateHandler, HandlerError.ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота
            var me = await botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
            await Console.Out.WriteLineAsync($"{me.FirstName} запущен!");
            await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
        }

        //private async static void BotOnMessageReceived(object? sender, MessageEventArgs e)
        //{
        //    var message = e.Message;
        //    var user = message.Chat;
        //    if (message == null || message.Type != MessageType.Text)
        //    {
        //        await botClient.SendTextMessageAsync(message.Chat, $"Извините, я пока что умею работать только с текстом, остальные вопросы к моему разработчикку @wolleY_47");
        //        return;
        //    }
        //    else if (string.Equals(message.Text, "погода", StringComparison.OrdinalIgnoreCase))
        //    {
        //        string jsonString;
        //        using (HttpClient client = new HttpClient())
        //        {
        //            jsonString = await client.GetStringAsync(url);
        //        }
        //        jsonString += "\n" + request2;
        //        jsonString = await ChatGptConnect(jsonString);
        //        await Console.Out.WriteLineAsync(jsonString);
        //        await botClient.SendTextMessageAsync(message.Chat, jsonString);
        //        return;
        //    }
        //    await Console.Out.WriteLineAsync($"first name:{message.Chat.FirstName}: {message.Text}");
        //    string result = await ChatGptConnect(message.Text);
        //    await Console.Out.WriteLineAsync(result);
        //    await botClient.SendTextMessageAsync(message.Chat, result);
        //}
        //private async static Task<string> ChatGptConnect(string message)
        //{
        //    string finnalyAnsver = String.Empty;
        //    OpenAIClient clientAI = new OpenAIClient("");
        //    var openAIResponse = await clientAI.GetChatCompletionsAsync("gpt-3.5-turbo-16k-0613", new ChatCompletionsOptions
        //    {
        //        Messages = { new ChatMessage(ChatRole.System, message) }
        //    });
        //    foreach (var item in openAIResponse.Value.Choices)
        //    {
        //        finnalyAnsver += item.Message.Content;
        //    }
        //    return finnalyAnsver ?? "";
        //}
    }
}