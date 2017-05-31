using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Utilities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlimGis.MapKit.Wpf.Scenes
{
    public class GpsScene : INotifyPropertyChanged
    {
        private bool isBusy;
        private bool isStopped;
        private ObservableCollection<GpsSprite> sprites;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<GpsSceneUpdatingEventArgs> Updating;
        public event EventHandler<GpsSceneUpdatedEventArgs> Updated;
        public event EventHandler<GpsSpriteArrivedEventArgs> Arrived;

        public GpsScene() : this(GeoUnit.Unknown)
        { }

        public GpsScene(GeoUnit unit)
        {
            Unit = unit;
            sprites = new ObservableCollection<GpsSprite>();
        }

        public int Fps { get; set; } = 32;

        public int SpeedUp { get; set; } = 10;

        public GeoUnit Unit { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value) return;
                isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        public ObservableCollection<GpsSprite> Sprites
        {
            get { return sprites; }
        }

        public async Task PlayAsync()
        {
            IsBusy = true;
            int suspendDuration = 1000 / Fps;

            int ellapsedDuration = 0;
            while (true)
            {
                int actualEllapsedDuration = ellapsedDuration * SpeedUp;

                var updatingArgs = new GpsSceneUpdatingEventArgs(actualEllapsedDuration);
                Updating?.Invoke(this, updatingArgs);
                if (updatingArgs.Handled) continue;

                foreach (var sprite in sprites)
                {
                    if (sprite.IsArrived) continue;

                    sprite.MoveByDuration(actualEllapsedDuration, Unit);
                    if (sprite.IsArrived)
                    {
                        GpsSpriteArrivedEventArgs e = new GpsSpriteArrivedEventArgs(sprite);
                        Arrived?.Invoke(this, e);
                    }
                }

                var updatedArgs = new GpsSceneUpdatedEventArgs(actualEllapsedDuration);
                Updated?.Invoke(this, updatedArgs);
                if (updatedArgs.Cancel) break;

                if (isStopped) break;
                if (sprites.All(s => s.IsArrived)) break;

                await Task.Run(() => Thread.Sleep(suspendDuration));
                ellapsedDuration += suspendDuration;
            }

            isStopped = false;
            IsBusy = false;
        }

        public void Stop()
        {
            isStopped = true;
        }

        internal void Reset()
        {
            Sprites.ForEach(s =>
            {
                s.IsArrived = false;
                s.Placement.Location = s.Route.Coordinates.First();
            });
        }
    }
}
