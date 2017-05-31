using SlimGis.MapKit.Geometries;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SlimGis.MapKit.Wpf.Scenes
{
    public class GpsSprite
    {
        private GeoCoordinate previousCoord;
        private RotateTransform rotateTrans;
        private bool isArrived;

        public GpsSprite() : this(null)
        { }

        public GpsSprite(Placement placement)
        {
            Placement = placement;
        }

        public Placement Placement { get; set; }

        public double Angle { get; set; }

        public double SpeedInKph { get; set; } = 80;

        public GeoLine Route { get; set; }

        public bool IsArrived
        {
            get { return isArrived; }
            set { isArrived = value; }
        }

        public async void MoveByDuration(int ellapsedDurationInMillisecond, GeoUnit unit)
        {
            if (Route == null || Placement == null) return;

            double ellapsedDurationInHour = ellapsedDurationInMillisecond / 3600000d;
            double drivenDistanceInKilometer = ellapsedDurationInHour * SpeedInKph;
            double routeDistance = Route.GetLength(unit, LengthUnit.Kilometer);

            if (drivenDistanceInKilometer > routeDistance)
            {
                drivenDistanceInKilometer = routeDistance;
                isArrived = true;

            }

            //GeoPoint currentPosition = await Task.Run(() => Route.GetPoint(drivenDistanceInKilometer, LengthUnit.Kilometer, unit));
            GeoPoint currentPosition = Route.GetPoint(drivenDistanceInKilometer, LengthUnit.Kilometer, unit);
            GeoCoordinate currentCoord = new GeoCoordinate(currentPosition);
            Placement.Location = currentCoord;

            if (previousCoord != null)
            {
                InitRotationTransform();

                double angle = -Math.Atan2(currentCoord.Y - previousCoord.Y, currentCoord.X - previousCoord.X) * 180 / Math.PI;
                angle = Math.Round(angle, 0);
                rotateTrans.Angle = angle;
            }

            previousCoord = currentCoord;
        }

        private void InitRotationTransform()
        {
            if (rotateTrans != null) return;

            if (Placement.RenderTransform is RotateTransform)
            {
                rotateTrans = (RotateTransform)Placement.RenderTransform;
            }
            else if (Placement.RenderTransform is TransformGroup)
            {
                var transGroup = (TransformGroup)Placement.RenderTransform;
                var tempRotateTrans = transGroup.Children.OfType<RotateTransform>().FirstOrDefault();
                rotateTrans = tempRotateTrans ?? new RotateTransform();
            }
            else if (Placement.RenderTransform != null)
            {
                var transGroup = new TransformGroup();
                transGroup.Children.Add(Placement.RenderTransform);
                transGroup.Children.Add((rotateTrans = new RotateTransform()));
                Placement.RenderTransform = transGroup;
            }
            else
            {
                Placement.RenderTransform = rotateTrans = new RotateTransform();
            }
        }
    }
}
