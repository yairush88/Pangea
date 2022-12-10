using Microsoft.Extensions.Logging;
using Pangea.Core.UI;

namespace Pangea.App.ViewModels
{
    public class ShellVM : ViewModelBase, IShellVM
    {
        private IMainWindowVM _mainWindowVM;

        public ShellVM()
        {
        }

        public ShellVM(IMainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
        }

        public IMainWindowVM MainWindowVM
        {
            get => _mainWindowVM;
            set
            {
                _mainWindowVM = value;
                SetProperty(ref _mainWindowVM, value);
            }
        }

        protected override void SetupCommands()
        {
            
        }

    }
}
