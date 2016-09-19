using System;
using System.Drawing;

namespace HelloGame
{
    public abstract class AThing
    {
        public ThingModel Model { get; set; }
        public BasicPhysics Physics { get; set; }

        protected readonly DateTime SpawnedAt;
        public TimeSpan Age { get { return DateTime.Now - SpawnedAt; } }
        public bool IsTimeToDie { get; protected set; }
        public TimeSpan TimeToLive { get; private set; }

        public AThing(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive;
            Model = new ThingModel();
            Physics = new BasicPhysics();
            SpawnedAt = DateTime.Now;
        }

        public abstract void PaintStuff(Graphics g);
        public abstract void UpdateModelInternal();

        public void UpdateModel()
        {
            if (TimeToLive.TotalMilliseconds > 0 && Age > TimeToLive)
            {
                IsTimeToDie = true;
            }
            else
            {
                UpdateModelInternal();

            }
        }

        public void Spawn(Point where, Real2DVector initialInertia)
        {
            Model.SetPosition(where);
            Physics.Interia = initialInertia;
        }
    }
}
