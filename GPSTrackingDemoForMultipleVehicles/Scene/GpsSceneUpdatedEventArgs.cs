using System;

namespace SlimGis.MapKit.Wpf.Scenes
{
    public class GpsSceneUpdatedEventArgs : EventArgs
    {
        private int ellapsedDurationInMillisecond;

        public GpsSceneUpdatedEventArgs(int ellapsedDurationInMillisecond)
        {
            this.ellapsedDurationInMillisecond = ellapsedDurationInMillisecond;
        }

        public int EllapsedDurationInMillisecond
        {
            get { return ellapsedDurationInMillisecond; }
            set { ellapsedDurationInMillisecond = value; }
        }

        public bool Cancel { get; set; }
    }
}
