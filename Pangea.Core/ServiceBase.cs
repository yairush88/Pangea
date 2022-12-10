using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SlimMessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace Pangea.Server
{
    public abstract class ServiceBase : IHostedService
    {
        protected IMessageBus _messageBus;

        public ServiceBase(IMessageBus messageBus, IServiceCollection services)
        {
            _messageBus = messageBus;
            AddServices(services);
        }
        
        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        protected virtual void AddServices(IServiceCollection services)
        {
        }
    }
}
