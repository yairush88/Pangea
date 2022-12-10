using Esri.ArcGISRuntime.UI.Controls;
using Pangea.App.ViewModels;
using System.Windows.Controls;

namespace Pangea.App.WPF.Views
{
    public partial class MapView : UserControl
    {
        private IMapVM _mapVM;
        public MapView()
        {
            InitializeComponent();
            mapView.Loaded += MapView_Loaded;
        }

        private void MapView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _mapVM = DataContext as IMapVM;
            _mapVM.Init(mapView);
        }

        private void MapView_GeoViewTapped(object sender, GeoViewInputEventArgs e)
        {
            _mapVM.OnGeoViewTapped(e);
        }
    }
}
