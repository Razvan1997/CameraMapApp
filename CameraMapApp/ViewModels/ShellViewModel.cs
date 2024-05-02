using Caliburn.Micro;
using CameraMapApp.Contracts.Views;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Portal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraMapApp.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<Screen>.Collection.AllActive, IShell
    {
        public MapViewModel MapViewModel { get; set; }
        public NavigationViewModel NavigationViewModel { get; set; }

        public ShellViewModel()
        {
            MapViewModel = new MapViewModel();
            NavigationViewModel = new NavigationViewModel();
            Items.AddRange(new Screen[] { MapViewModel, NavigationViewModel });
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
