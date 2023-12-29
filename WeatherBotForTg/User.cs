using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace WeatherBotForTg
{
    public class User
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string FirstName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public TimeOnly SendWeatherTime { get; set; }

    }
}
