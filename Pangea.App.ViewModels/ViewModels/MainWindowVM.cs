using Pangea.Core.UI;

namespace Pangea.App.ViewModels
{
    public class MainWindowVM : ViewModelBase, IMainWindowVM
    {
        public MainWindowVM()
        {

        }

        public MainWindowVM(IMessagesVM messagesVM, IMapVM mapVM)
        {
            MessagesVM = messagesVM;
            MapVM = mapVM;
        }

        private IMapVM _mapVM;
        public IMapVM MapVM
        {
            get => _mapVM;
            set => SetProperty(ref _mapVM, value);
        }


        private IMessagesVM _messagesVM;
        public IMessagesVM MessagesVM
        {
            get => _messagesVM;
            set => SetProperty(ref _messagesVM, value);
        }

        private ISceneVM _sceneVM;
        public ISceneVM SceneVM
        {
            get => _sceneVM;
            set => SetProperty(ref _sceneVM, value);
        }

        protected override void SetupCommands()
        {
        }
    }
}
