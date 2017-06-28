Keywords: gps tracking, vehicle tracking, vehicle moving animation, simulate gps

# Animated GPS Tracking with Multiple Vehicles

Are you looking for a prototype that could track GPS signal of vehicles? The vehicles move with animation on their own speed and want to control the speed of GPS scene.

[This project](https://github.com/SlimGIS/GPSTrackingDemoForMultipleVehicles-Wpf) represents what you are looking for. First let's see a [video on YouTube](https://youtu.be/vbf1wP0IMcw) about it. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/vbf1wP0IMcw" frameborder="0" allowfullscreen></iframe>

How the magic works? Please follow this guide and you could implement your own tracking system.

### GPS Scene
The first class that I want to introduce is GPS scene. This class could help to create the animation scene for GPS tracking. It contains following useful members.

- __Fps__ means `frames per second`. In another word, that reflects how many times we refresh in a single second. For instance, we usually say 60 FPS, that means the animation refreshes `1000 / 60 = 16.6667` million seconds a time. Our default value is 32 frames per second. That could saves time that we used to update the vehicles location in one frame.
- __SpeedUp__ may be `Acceleration` is a better name, we will consider to rename it in the next build. You know the scene is a virtual world for GPS tracking. For example, if you are watching a vehicle moving on a map, it might moves slowly like a tortoise. In this case, we want to speeds up the virtual world so that the vehicle moves faster visually, but it actually moves with its own real speed. By default, we make it 10 times faster than the real world.
- __Sprites__ this collection maintains all the vehicles we want to track. We will introduce it in the next section.
- __PlayAsync()__ is an async method to tell the vehicles to move with its pre-defined route line. It runs async which means it doesn't block the main thread, like panning or zooming the map.
- __Stop()__ means all the moving vehicles stop at its current location.
> I think I missed the Pause() method in the API. It is not actually used in my current project. If you really need it, please feel free to contact us by support@slimgis.com. We will consider to add it in the next build.

### GPS Sprite
`Sprite` is a popular name in animation world. Like `SpriteKit`, `SKSprite` on iOS etc. So I choose this name. Just like this name, a `Sprite` moves here and there. In GPS tracking scene, we wrap every moving thing to a `GpsSprite`, so that we can make our static placements move as you need. It contains following useful members.

- __Placement__ is a base class of all our placements. Like marker, popup are all inherited from this class. With this property, we could define a marker or popup to the sprite, so that it could looks like a vehicle or anything else.
- __Angle__ is used as to define the starting angle of the marker image. By default it is 0 which means the marker's image, for example, a vehicle should head to east. If its image is head to north, we need to adjust this angle, so that our scene could turn the vehicle's head to the right direction during moving.
- __SpeedInKph__ means the speed of the sprite in kilometers per hour. Pretty easy to understand.
- __Route__ is the routing line that a sprite will move along with. The sprite will stop at the end of the route, if the scene or the other sprites are still moving.
- __IsArrived__ indicates if current sprites is arrived to the end of the pre-defined route line.

With this guide and the project we have, I think it is enough for you to build your GPS tracking world. Any suggestions, we are glad to hear. Please don't hesitate to contact us by support@slimgis.com.

## Related Resources
- [Source code](https://github.com/SlimGIS/GPSTrackingDemoForMultipleVehicles-Wpf)
- [Video](https://youtu.be/vbf1wP0IMcw)
- [Vehicle tracking head north sample](https://slimgis.com/documents/gps-tracking-wpf)
