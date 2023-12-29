using Telegram.Bot.Types;

namespace WeatherBotForTg.Service
{
    public class UserCRUDServices
    {
        public static async Task UserCreate(Update update)
        {
            var user = new User();
            var informationAboutUser = update.Message.Chat;
            if (!CheckService.ChekUser(informationAboutUser.Id))
                return;
            using (ApplicationContext _db = new())
            {
                user.UserId = informationAboutUser.Id;
                user.FirstName = informationAboutUser.FirstName;
                user.UserName = informationAboutUser.Username;
                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }
        public static async Task UserUpdataSendWeatherTime(Update update, TimeOnly sendWeatherTime)
        {
            var user = new User();
            var informationAboutUser = update.Message.Chat;
            using (ApplicationContext _db = new())
            {
                user.UserId = informationAboutUser.Id;
                user.UserName = informationAboutUser.FirstName;
                user.UserName = informationAboutUser.Username;
                user.SendWeatherTime = sendWeatherTime;
                _db.Update(user);
                _db.SaveChanges();
            }
        }
        public static async Task UserUpdataLocation(Update update, double latitude, double longitude)
        {
            var user = new User();
            var informationAboutUser = update.Message.Chat;
            using (ApplicationContext _db = new())
            {
                user.UserId = informationAboutUser.Id;
                user.FirstName = informationAboutUser.FirstName;
                user.UserName = informationAboutUser.Username;
                user.Latitude = latitude;
                user.Longitude = longitude;
                _db.Update(user);
                _db.SaveChanges();
            }
        }
    }
}
