namespace WeatherBotForTg.Model
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
