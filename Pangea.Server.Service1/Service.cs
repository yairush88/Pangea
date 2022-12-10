using Microsoft.Extensions.DependencyInjection;
using Pangea.Server.Common.Messages;
using SlimMessageBus;

namespace Pangea.Server.Service1
{
    public class ServiceMain1 : ServiceBase
    {
        public ServiceMain1(IMessageBus messageBus, IServiceCollection services) : base(messageBus, services)
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var msg = new Service1StatusMessage { Message = "Ready to start" };
            await _messageBus.Publish(msg, cancellationToken: cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override void AddServices(IServiceCollection services)
        {
            base.AddServices(services);
            //services
            //    .AddMessageBusConsumersFromAssembly(mbb =>
            //    {
            //        mbb
            //        .WithProviderMemory()
            //        //.Produce<StatusMessage>(builder => builder.DefaultTopic("status1"))
            //        //.Consume<StatusMessage>(builder => builder
            //        //    .Topic("status1")
            //        //    .WithConsumer<StatusMessageHandler>());
            //    });
        }
    }
}
