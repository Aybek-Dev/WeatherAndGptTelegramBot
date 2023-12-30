using Telegram.Bot;
using Telegram.Bot.Exceptions;

namespace WeatherBotForTg
{
    public class HandlerError
    {
        public static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };
            botClient.SendTextMessageAsync(801654363, ErrorMessage);
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
