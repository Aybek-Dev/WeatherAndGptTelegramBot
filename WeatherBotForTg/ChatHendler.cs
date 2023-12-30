using Npgsql.Replication.PgOutput.Messages;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WeatherBotForTg.Service;

namespace WeatherBotForTg
{
    public class ChatHendler
    {
        

        public static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                        var message = update.Message;
                        var user = message.From;
                        await Console.Out.WriteLineAsync($"UserId: {user.Id} | FirstName: {user.FirstName} | UserName: {user.Username}");
                        await Console.Out.WriteLineAsync($"Message: {message.Text}");
                        switch (message.Type)
                        {
                            case MessageType.Text:
                                if (message.Text == "/start")
                                {
                                    await UserCRUDServices.UserCreate(update);
                                    await botClient.SendTextMessageAsync(
                                        user.Id,
                                        "Приветствую тебя 😊\r\nЯ помогу тебе получать  уведомления о погоде, с моей помощью ты всегда будешь в курсе ситуации \U0001f929\r\nТакже, я работаю на основе искусственного интеллекта и могу помочь с твоими задачами, а также поддержать разговор🤓");
                                    var replyKeyboard = new ReplyKeyboardMarkup(
                                    new List<KeyboardButton[]>()
                                    {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Установить координаты📍"){ RequestLocation = true},
                                            new KeyboardButton("Часы оповещения🕘"),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton("Chat Gpt"),
                                            new KeyboardButton("Погода☀️")
                                        }
                                    })
                                    {
                                        ResizeKeyboard = true,
                                    };
                                    await botClient.SendTextMessageAsync(
                                        user.Id,
                                        "Привет! Выберите действие:",
                                        replyMarkup: replyKeyboard);
                                    return;
                                }
                                else if (message.Text == "Погода☀️")
                                {
                                    _ = ShowWeather.ShowWeatherGpt(update, botClient);
                                }
                                else if (message.Text == "Часы оповещения🕘")
                                {
                                    return;
                                }
                                else if (message.Text == "Chat Gpt")
                                {
                                    _ = botClient.SendTextMessageAsync(user.Id, "Слушаю вас внимательно");
                                }
                                else
                                {
                                    _ =Task.Run(async () =>
                                    {
                                        var result = await СhatGptRequest.ConnectFromGpt(message.Text);
                                        await botClient.SendTextMessageAsync(user.Id, result);
                                    });
                                }
                                break;
                            case MessageType.Location:
                                double latitude = message.Location.Latitude;
                                double longitude = message.Location.Longitude;
                                _ = UserCRUDServices.UserUpdataLocation(update, latitude, longitude);
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }
        }
    }
}

