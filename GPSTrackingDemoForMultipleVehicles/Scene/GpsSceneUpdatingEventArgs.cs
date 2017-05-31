using System;

namespace SlimGis.MapKit.Wpf.Scenes
{
    public class GpsSceneUpdatingEventArgs : EventArgs
    {
        private int ellapsedDurationInMillisecond;

        public GpsSceneUpdatingEventArgs(int ellapsedDurationInMillisecond)
        {
            this.ellapsedDurationInMillisecond = ellapsedDurationInMillisecond;
        }

        public int EllapsedDurationInMillisecond
        {
            get { return ellapsedDurationInMillisecond; }
            set { ellapsedDurationInMillisecond = value; }
        }

        public bool Handled { get; set; }
    }
}
