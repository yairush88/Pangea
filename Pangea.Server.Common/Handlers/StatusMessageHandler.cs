using Pangea.Server.Common.Messages;
using SlimMessageBus;

namespace Pangea.Server.Common.Handlers
{
    internal class StatusMessageHandler : IConsumer<Service1StatusMessage>
    {
        public Task OnHandle(Service1StatusMessage message, string path)
        {
            return Task.CompletedTask;
        }
    }
}