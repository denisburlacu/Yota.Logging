// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yota.Logging;

var serviceCollection = new ServiceCollection();

serviceCollection.AddYotaLogging(it =>
{
    it.ToConsole()
        .ToElastic("home", "http://elastic:Nsjn4dxi8AWWwZcAAtF6@test.com");
});

var provider = serviceCollection.BuildServiceProvider();
var logger = provider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Hoho");

Console.WriteLine("Hello, World!");