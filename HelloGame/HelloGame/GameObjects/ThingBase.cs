using System;
using System.Drawing;
using HelloGame.MathStuff;
using System.Collections.Generic;

namespace HelloGame.GameObjects
{
    public class ThingSettings
    {
        public TimeSpan TimeToLive { get; set; }
        public decimal Aerodynamism { get; set; }
        public decimal Mass { get; set; }
        public decimal RadPerSecond { get; set; }
    }

    public abstract class ThingBase : ElapsingThing
    {
        public AlmostPhysics Physics { get; }
        public ThingBase Creator { get; }

        protected ThingBase(ThingSettings settings, ThingBase creator = null) : base(settings.TimeToLive)
        {
            Physics = new AlmostPhysics(settings.Aerodynamism);
            Creator = creator;
            Physics.Mass = settings.Mass;
            Physics.RadPerSecond = settings.RadPerSecond;
        }

        public abstract void PaintStuff(Graphics g);
        protected abstract void UpdateModelInternal(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings);
        public bool IsDestroyed { get; protected set; }

        public void Destroy()
        {
            IsDestroyed = true;
        }

        public void UpdateModel(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            UpdateElapsing();
            if (!IsTimeToElapse)
            {
                // Update stuff like propelling.
                UpdateModelInternal(timeSinceLastUpdate, otherThings);

                // Add the propeller force to the interia.
                Physics.Interia.Add(Physics.SelfPropelling);

                // Drag changes the inertia?
                Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism * 0.05m);
                Physics.Interia.Add(Physics.Drag);

                Real2DVector totalForce = Physics.TotalForce;

                // No mass? No gravity. Think lazer.
                if (Physics.Mass == 0)
                {
                    // No mass? 
                    Physics.PositionX += totalForce.X;
                    Physics.PositionY += totalForce.Y;
                }
                else
                {
                    // Calculate gravity.
                    Real2DVector gravity = CalculateGravity(otherThings);
                    totalForce.Add(gravity);

                    // Move the object.
                    Physics.PositionX += totalForce.X / Physics.Mass;
                    Physics.PositionY += totalForce.Y / Physics.Mass;
                }
            }
        }

        private Real2DVector CalculateGravity(List<ThingBase> otherThings)
        {
            Real2DVector result = new Real2DVector();

            foreach (ThingBase thing in otherThings)
            {
                if (thing == this) { continue; }
                var distance = thing.DistanceTo(this);
                if (distance == 0) { continue; }

                decimal length = 0.01m * Physics.Mass * thing.Physics.Mass / (decimal)Math.Pow((double)distance,2);
                var grav = new Real2DVector();

                var x = thing.Physics.PositionX - Physics.PositionX;
                var y = thing.Physics.PositionY - Physics.PositionY;
                grav.X = x;
                grav.Y = y;

                grav.Set(grav.Angle, length);

                result.Add(grav);
            }
            return result;
        }

        public abstract void CollidesWith(ThingBase other);

        public decimal DistanceTo(ThingBase another)
        {
            var x1 = Physics.PositionX;
            var x2 = another.Physics.PositionX;
            var y1 = Physics.PositionY;
            var y2 = another.Physics.PositionY;
            return (decimal)Math.Sqrt(Math.Pow((double)(x1 - x2), 2) + Math.Pow((double)(y1 - y2), 2));
        }

        public void Spawn(Point where, Real2DVector initialInertia = null)
        {
            Physics.SetPosition(where);
            Physics.Interia = initialInertia ?? new Real2DVector();
        }
    }
}
