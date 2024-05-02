using Caliburn.Micro;
using CameraMapApp.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CameraMapApp.Services
{
    [Export(typeof(IViewLocator))]
    public class ViewLocator : IViewLocator
    {
        private readonly IThemeManager themeManager;

        [ImportingConstructor]
        public ViewLocator(IThemeManager themeManager)
        {
            this.themeManager = themeManager;
        }

        public UIElement GetOrCreateViewType(Type viewType)

        {
            var cached = IoC.GetAllInstances(viewType).OfType<UIElement>().FirstOrDefault();
            if (cached != null)
            {
                Caliburn.Micro.ViewLocator.InitializeComponent(cached);
                return cached;
            }

            if (viewType.IsInterface || viewType.IsAbstract || !typeof(UIElement).IsAssignableFrom(viewType))
            {
                return new TextBlock { Text = $"Cannot create {viewType.FullName}." };
            }

            var newInstance = (UIElement)Activator.CreateInstance(viewType)!;

            Caliburn.Micro.ViewLocator.InitializeComponent(newInstance);

            // alternative way to use the MahApps resources
            // (newInstance as Window)?.Resources.MergedDictionaries.Add(themeManager.GetThemeResources());

            return newInstance;
        }
    }
}
