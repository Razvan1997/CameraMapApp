using Caliburn.Micro;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CameraMapApp.ViewModels
{
    public class NavigationViewModel : Screen
    {
        public NavigationViewModel()
        {

        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
           
            return Task.FromResult(true);
        }
    }
}
