using Caliburn.Micro;
using CameraMapApp.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CameraMapApp.Startup
{
    public delegate void StartupTask();

    public class StartupTasks
    {
        private readonly IServiceLocator serviceLocator;

        [ImportingConstructor]
        public StartupTasks(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        [Export(typeof(StartupTask))]
        public void ApplyBindingScopeOverride()
        {
            //var getNamedElements = BindingScope.GetNamedElements;
            //BindingScope.GetNamedElements = o =>
            //{
            //    var metroWindow = o as MetroWindow;
            //    if (metroWindow == null)
            //    {
            //        return getNamedElements(o);
            //    }

            //    var list = new List<FrameworkElement>(getNamedElements(o));
            //    var type = o.GetType();
            //    var fields =
            //        o.GetType()
            //         .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            //         .Where(f => f.DeclaringType == type);
            //    var flyouts =
            //        fields.Where(f => f.FieldType == typeof(FlyoutsControl))
            //              .Select(f => f.GetValue(o))
            //              .Cast<FlyoutsControl>();
            //    list.AddRange(flyouts);
            //    return list;
            //};
        }

        [Export(typeof(StartupTask))]
        public void ApplyViewLocatorOverride()
        {
            var themeManager = this.serviceLocator.GetInstance<IThemeManager>();
            if (themeManager is not null)
            {
                Application.Current.Resources.MergedDictionaries.Add(themeManager.GetThemeResources());
            }

            var viewLocator = this.serviceLocator.GetInstance<IViewLocator>();
            if (viewLocator is not null)
            {
                Caliburn.Micro.ViewLocator.GetOrCreateViewType = viewLocator.GetOrCreateViewType;
            }

            var arGisSdk = this.serviceLocator.GetInstance<IArcGisSdkCreate>();
            arGisSdk.Initialize();
        }
    }
}
