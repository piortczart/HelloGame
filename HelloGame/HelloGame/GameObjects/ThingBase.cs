using System;
using System.Drawing;
using HelloGame.MathStuff;

namespace HelloGame.GameObjects
{
    public abstract class ThingBase : ElapsingThing
    {
        public ThingModel Model { get; }
        public BasicPhysics Physics { get; }

        protected ThingBase(TimeSpan timeToLive) : base(timeToLive)
        {
            Model = new ThingModel();
            Physics = new BasicPhysics();
        }

        public abstract void PaintStuff(Graphics g);
        protected abstract void UpdateModelInternal();

        public void UpdateModel()
        {
            UpdateElapsing();
            if (!IsTimeToDie)
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
