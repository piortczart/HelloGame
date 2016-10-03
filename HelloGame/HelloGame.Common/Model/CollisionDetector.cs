namespace HelloGame.Common.Model
{
    public class CollisionDetector
    {
        public readonly EventPerSecond CollisoinsCounter = new EventPerSecond();

        public void DetectCollisions(ThingBase[] things)
        {
            CollisoinsCounter.Add();

            for (int i = 0; i < things.Length; i++)
            {
                ThingBase thing1 = things[i];
                for (int j = i + 1; j < things.Length; j++)
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
