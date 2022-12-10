using Microsoft.Extensions.Options;

namespace Pangea.App.ViewModels.Services
{
    public class MessageService : IMessageService
    {
        public MessageService(IOptions<Settings> options)
        {
            
        }

        public void GetMessage(int id)
        {

        }
    }
}