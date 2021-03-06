﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyCompany.Crm
{
    public static class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseStartup<Startup>())
            .UseDefaultServiceProvider((context, options) =>
            {
                var isNotProduction = !context.HostingEnvironment.IsProduction();
                options.ValidateScopes = isNotProduction;
                options.ValidateOnBuild = isNotProduction;
            });
    }
}