using System;

namespace SlimGis.MapKit.Wpf.Scenes
{
    public class GpsSpriteArrivedEventArgs : EventArgs
    {
        public GpsSpriteArrivedEventArgs(GpsSprite sprite)
        {
            Sprite = sprite;
        }

        public GpsSprite Sprite { get; set; }
    }
}
