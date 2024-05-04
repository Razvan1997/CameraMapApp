using Caliburn.Micro;
using CameraMapApp.Contracts.Views;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Esri.ArcGISRuntime.UI.Controls;
using CameraMapApp.Views;

namespace CameraMapApp.ViewModels
{
    public class MapViewModel : Screen, IMap
    {
        private Esri.ArcGISRuntime.UI.Controls.MapView _myMapView;
        public Esri.ArcGISRuntime.UI.Controls.MapView MyMapView
        {
            get => _myMapView;
            set => Set(ref _myMapView, value);
        }

        private const string FeatureServiceUrl = "https://sampleserver6.arcgisonline.com/arcgis/rest/services/DamageAssessment/FeatureServer/0";

        // Name of the field that will be updated.
        private const string AttributeFieldName = "typdamage";

        // Hold a reference to the feature layer.
        private FeatureLayer _damageLayer;

        // Hold a reference to the feature table.
        private ServiceFeatureTable _damageFeatureTable;

        // Hold a reference to the selected feature.
        private ArcGISFeature _selectedFeature;

        // Create a button for deleting features.
        private Button _deleteButton;


        public MapViewModel()
        {

        }

        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            MyMapView = new Esri.ArcGISRuntime.UI.Controls.MapView();
            MyMapView.Map = new Map(BasemapStyle.ArcGISStreets);

            InitilizeFeature();

            return Task.FromResult(true);
        }

        private async void InitilizeFeature()
        {
            try
            {
                // Create a service geodatabase from the feature service.
                var serviceGeodatabase = new ServiceGeodatabase(new Uri(FeatureServiceUrl));
                await serviceGeodatabase.LoadAsync();

                // Get the feature table from the service geodatabase referencing the Damage Assessment feature service.
                // Creating the feature table from the feature service will cause the service geodatabase to be null.
                _damageFeatureTable = serviceGeodatabase.GetTable(0);

                // Update attributes - when the table loads, use it to discover the domain of the typdamage field.
                _damageFeatureTable.Loaded += DamageTable_Loaded;

                // Create a feature layer to visualize the features in the table.
                _damageLayer = new FeatureLayer(_damageFeatureTable);

                // Add the layer to the map.
                MyMapView.Map.OperationalLayers.Add(_damageLayer);

                // Create a button for deleting the feature.
                _deleteButton = new Button
                {
                    Content = "Delete incident",
                    Padding = new Thickness(5)
                };

                // Handle button clicks.
                _deleteButton.Click += DeleteButton_Click;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

            MyMapView.GeoViewTapped += MapView_Tapped_CreateFeature;

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void DamageTable_Loaded(object? sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void MapView_Tapped_CreateFeature(object sender, GeoViewInputEventArgs e)
        {
            try
            {
                // Create the feature.
                var feature = (ArcGISFeature)_damageFeatureTable.CreateFeature();

                // Get the normalized geometry for the tapped location and use it as the feature's geometry.
                var tappedPoint = (MapPoint)GeometryEngine.NormalizeCentralMeridian(e.Location);
                feature.Geometry = tappedPoint;

                // Set feature attributes.
                feature.SetAttributeValue("typdamage", "Minor");
                feature.SetAttributeValue("primcause", "Earthquake");

                // Add the feature to the table.
                await _damageFeatureTable.AddFeatureAsync(feature);

                // Apply the edits to the service on the service geodatabase.
                await _damageFeatureTable.ServiceGeodatabase.ApplyEditsAsync();

                // Update the feature to get the updated objectid - a temporary ID is used before the feature is added.
                feature.Refresh();

                // Confirm feature addition.
                MessageBox.Show("Created feature " + feature.Attributes["objectid"], "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

    }
}
