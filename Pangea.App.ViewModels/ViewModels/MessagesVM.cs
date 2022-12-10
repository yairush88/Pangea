using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Toolkit.Mvvm.Input;
using Pangea.App.ViewModels.Services;
using System.Windows.Input;

namespace Pangea.App.ViewModels
{
    public class MessagesVM : IMessagesVM
    {
        private readonly IOptions<Settings> _options;

        public MessagesVM(IMessageService messageService, IOptions<Settings> options, IConfiguration configuration)
        {
            LoadMessagesCommand = new RelayCommand(LoadMessages);
            _options = options;
        }

        private void LoadMessages()
        {
            var version = _options.Value.Version;
        }

        public ICommand LoadMessagesCommand { get; set; }
    }
}
