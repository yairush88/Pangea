using Esri.ArcGISRuntime.UI.Controls;
using Pangea.Core.UI;
using System.Threading.Tasks;

namespace Pangea.App.ViewModels
{
    public interface IMapVM
    {
        IMap2D Map { get; }

        Task Init(object mapView);
        void OnGeoViewTapped(object arg);
    }
}
