using System.Windows.Input;

namespace Pangea.App.ViewModels
{
    public interface IMessagesVM
    {
        public ICommand LoadMessagesCommand { get; set; }
    }
}