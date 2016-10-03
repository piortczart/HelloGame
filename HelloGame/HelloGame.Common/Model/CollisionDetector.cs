using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;
using System.Collections.Generic;

namespace HelloGame.Common.Model
{
    public class CollisionDetector
    {
        public EventPerSecond CollisoinsCounter = new EventPerSecond();

        public void DetectCollisions(List<ThingBase> things)
        {
            CollisoinsCounter.Add();

            for (int i = 0; i < things.Count; i++)
            {
                ThingBase thing1 = things[i];
                for (int j = i + 1; j < things.Count; j++)
                {
                    ThingBase thing2 = things[j];

                    if (thing1.DistanceTo(thing2) <= 3)
                    {
                        thing1.CollidesWith(thing2);
                        thing2.CollidesWith(thing1);
                    }
                }
            }
        }
    }
}
