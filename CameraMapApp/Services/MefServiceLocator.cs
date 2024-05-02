using CameraMapApp.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro;

namespace CameraMapApp.Services
{
    [Export(typeof(IServiceLocator))]
    public class MefServiceLocator : IServiceLocator
    {
        private readonly CompositionContainer compositionContainer;

        [ImportingConstructor]
        public MefServiceLocator(CompositionContainer compositionContainer)
        {
            this.compositionContainer = compositionContainer;
        }

        public T? GetInstance<T>()
            where T : class
        {
            try
            {
                return this.compositionContainer.GetExportedValue<T>();
            }
            catch (Exception exception)
            {
                throw new MahAppsException($"Could not locate any instances of contract {typeof(T)}.", exception);
            }
        }
    }
}
