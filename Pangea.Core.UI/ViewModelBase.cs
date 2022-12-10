using CommunityToolkit.Mvvm.ComponentModel;

namespace Pangea.Core.UI
{
    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            // TODO: Add logging
            SetupCommands();
        }

        protected abstract void SetupCommands();
    }
}
