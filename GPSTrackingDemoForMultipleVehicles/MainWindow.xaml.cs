using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.Wpf;
using SlimGis.MapKit.Wpf.Scenes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GPSTrackingDemoForMultipleVehicles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string[] vehicleColors = new[] { "blue", "green", "orange", "red", "yellow" };

        private GpsScene gpsScene;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WpfMap_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            gpsScene = new GpsScene(Map1.MapUnit);

            Map1.UseOpenStreetMapAsBaseMap();

            var routeLayer = new MemoryLayer();
            routeLayer.Styles.Add(new LineStyle(GeoColor.FromRgba(GeoColors.SkyBlue, 180), 10));
            Map1.AddStaticLayers("Route Line", routeLayer);

            var routeDataStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/routes.txt")).Stream;
            var speedRandom = new Random();
            using (var streamReader = new StreamReader(routeDataStream))
            {
                var routeData = streamReader.ReadToEnd().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < routeData.Length; i++)
                {
                    GeoLine route = new GeoLine(routeData[i]);
                    routeLayer.Features.Add(new Feature(route));
                    Marker vehicle = GetVehicle(i, route);
                    Map1.Placements.Add(vehicle);

                    GpsSprite gpsSprite = new GpsSprite(vehicle);
                    gpsScene.Fps = 60;
                    gpsSprite.SpeedInKph = speedRandom.Next(40, 120);
                    gpsSprite.Route = route;
                    gpsScene.Sprites.Add(gpsSprite);
                }
            }

            GeoBound bound = routeLayer.GetBound();
            bound.ScaleUp(40);
            Map1.ZoomTo(bound);
        }

        private static Marker GetVehicle(int index, GeoLine route)
        {
            Marker vehicle = new Marker();
            vehicle.DropShadow = false;
            vehicle.OffsetY = 4;
            vehicle.RenderTransform = new RotateTransform { CenterX = 17, CenterY = 7 };
            vehicle.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/Resources/vehicle-{vehicleColors[index % vehicleColors.Length]}.png", UriKind.RelativeOrAbsolute));
            vehicle.Location = route.GetVertices().First();
            return vehicle;
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;

            gpsScene.Reset();
            await gpsScene.PlayAsync();
            PlayButton.IsEnabled = true;
        }
    }
}
