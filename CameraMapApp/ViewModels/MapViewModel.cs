using Caliburn.Micro;
using CameraMapApp.Contracts.Views;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraMapApp.ViewModels
{
    public class MapViewModel : Screen, IMap
    {
        private Map _map;
        public Map Map
        {
            get => _map;
            set => Set(ref _map, value);
        }

        public MapViewModel()
        {

        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            _map = new Map(BasemapStyle.ArcGISStreetsNight)
            {
                InitialViewpoint = new Viewpoint(new Envelope(-180, -85, 180, 85, SpatialReferences.Wgs84)),
                //Basemap = new Basemap(BasemapStyle.ArcGISDarkGray)
            };

            return Task.FromResult(true);
        }
    }
}
