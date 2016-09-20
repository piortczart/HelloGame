﻿using HelloGame.GameObjects;
using System.Collections.Generic;

namespace HelloGame
{
    public class CollisionDetector
    {
        public void DetectCollisions(List<ThingBase> things)
        {
            for (int i = 0; i < things.Count; i++)
            {
                ThingBase thing1 = things[i];
                for (int j = i + 1; j < things.Count; j++)
                {
                    ThingBase thing2 = things[j];

                    if (thing1.DistanceTo(thing2) < 10)
                    {
                        thing1.CollidesWith(thing2);
                        thing2.CollidesWith(thing1);
                    }
                }
            }
        }
    }
}