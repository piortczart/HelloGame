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
        protected abstract void UpdateModelInternal(TimeSpan timeSinceLastUpdate);

        public void UpdateModel(TimeSpan timeSinceLastUpdate)
        {
            UpdateElapsing();
            if (!IsTimeToDie)
            {
                // Update stuff like propelling.
                UpdateModelInternal(timeSinceLastUpdate);

                // Add the propeller force to the interia.
                Physics.Interia.Add(Physics.SelfPropelling);

                // Drag changes the inertia?
                Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism * 0.05m);

                Physics.Interia.Add(Physics.Drag);

                // Move the object.
                Real2DVector totalForce = Physics.TotalForce;
                Model.PositionX += totalForce.X / 10;
                Model.PositionY += totalForce.Y / 10;

            }
        }

        public void Spawn(Point where, Real2DVector initialInertia)
        {
            Model.SetPosition(where);
            Physics.Interia = initialInertia;
        }
    }
}
