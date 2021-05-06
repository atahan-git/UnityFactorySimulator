# UnityFactorySimulator

This is the core of the factory simulation I'm using for my game [Made in Mars](http://madeinmarsgame.com/), released for free for using as a starting point in your projects!

You can see the code is fully unit tested, and the code includes a simple ASCII renderer.

The belt system in this factory works as follows:
* Connectors transfer items between belts and buildings (also belt to belt and building to building). They can only be linear, and cannot transfer from connector to connector.
* Belts moves items forward. They can only be linear.
* Buildings check for their input slots each tick, and craft the item and put it to the output slot if they can.

![Example](/ASCIIFactory.gif)

Hope this is useful! You can use this as a starting point in any game you want, as long as you list me somewhere! Check the license for more details.
