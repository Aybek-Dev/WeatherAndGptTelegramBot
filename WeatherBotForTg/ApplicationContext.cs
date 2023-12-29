using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherBotForTg
{
    public class ApplicationContext:DbContext
    {
        string configFotConnect;
        public DbSet<User> Users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configFotConnect = File.ReadAllText(@"C:\Users\AYBEK\Desktop\C# Home task\WeatherBotForTg\WeatherBotForTg\Configuration\ConfigForConnectDb.txt");
            optionsBuilder.UseNpgsql(configFotConnect);
        }
    }
}
