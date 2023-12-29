using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WeatherBotForTg.Service
{
    public class CheckService
    {
        public static bool ChekUser(long id)
        {
            using (ApplicationContext _db = new())
            {
                foreach(var user in _db.Users)
                {
                    if(user.UserId==id)
                        return false;
                };
            }
            return true;
        }
        public static async Task<(double Latitude, double Longitude)?> CheckUserLocation(long id)
        {
            using (ApplicationContext _db = new())
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                {
                    return null;
                }

                return (user.Latitude, user.Longitude);
            }
        }

    }
}
