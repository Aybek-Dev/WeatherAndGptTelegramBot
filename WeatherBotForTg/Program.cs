using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace WeatherBotForTg
{
    class Program
    {
        private static ReceiverOptions? _receiverOptions;
        static private string tokek { get; set; } = File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\BotToken.txt");
        private static ITelegramBotClient? botClient;
        static async Task Main(string[] args)
        {
            botClient = new TelegramBotClient(tokek); 
            _receiverOptions = new ReceiverOptions // присваем значение настройкам бота
            {
                AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
                {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
                ThrowPendingUpdates = true,
            };
            using var cts = new CancellationTokenSource();
            // UpdateHander - обработчик приходящих Update`ов
            // ErrorHandler - обработчик ошибок, связанных с Bot API

            botClient.StartReceiving(
                (botClient, update, ct) =>
                    {
                        _ = Task.Run(async () => await ChatHendler.UpdateHandler(botClient, update, ct), ct);
                        return Task.CompletedTask;
                    },
                HandlerError.ErrorHandler,
                _receiverOptions,
                cts.Token
             );     // Запускаем бота
            var me = await botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
            await Console.Out.WriteLineAsync($"{me.FirstName} запущен!\U0001f929");
            await Task.Delay(-1);
        }
    }
}