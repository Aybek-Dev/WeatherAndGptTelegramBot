using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace WeatherBotForTg.Exceptions
{
    public class ExceptionHTTPResponse : Exception
    {   
        public ExceptionHTTPResponse(string message)
        : base(message)
        {
        }
    }
}
