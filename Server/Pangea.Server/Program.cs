using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pangea.Server.Service1;
using Pangea.Server.Service2;
using SlimMessageBus.Host.Memory;
using SlimMessageBus.Host.MsDependencyInjection;
using System;
using System.Threading.Tasks;

namespace Pangea.Server
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<ServiceMain1>()
                        .AddHostedService<ServiceMain2>();
                        //.AddSlimMessageBus(mbb =>
                        //{
                        //    mbb
                        //    .WithProviderMemory()
                        //    .Produce<Service1StatusMessage>(builder => builder.DefaultTopic("status1"))
                        //    .Consume<Service1StatusMessage>(builder => builder
                        //        .Topic("status1")
                        //        .WithConsumer<StatusMessageHandler>());
                        //},

                        //addConsumersFromAssembly: new[] { typeof(StatusMessageHandler).Assembly });

                }).Build();

            await host.RunAsync();
        }
    }
}
