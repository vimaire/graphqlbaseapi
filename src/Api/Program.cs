using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Oakton.AspNetCore;
[assembly: Oakton.OaktonCommandAssembly]

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .RunOaktonCommands(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
