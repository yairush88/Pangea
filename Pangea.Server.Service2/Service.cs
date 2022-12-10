using Microsoft.Extensions.DependencyInjection;
using SlimMessageBus;

namespace Pangea.Server.Service2
{
    public class ServiceMain2 : ServiceBase
    {
        public ServiceMain2(IMessageBus messageBus, IServiceCollection services) : base(messageBus, services)
        {
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
