using Pangea.App.ViewModels;
using System.Windows;

namespace Pangea.App.WPF
{
    public partial class ShellView : Window
    {
        public ShellView()
        {

        }

        public ShellView(IShellVM shellVM)
        {
            InitializeComponent();
            this.DataContext = shellVM;
        }
    }
}
