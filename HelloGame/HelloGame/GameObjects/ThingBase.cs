using System;
using System.Drawing;
using HelloGame.MathStuff;
using System.Collections.Generic;
using HelloGame.Physicsish;

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
        protected bool IsDestroyed { get; private set; }

        protected void Destroy()
        {
            IsDestroyed = true;
        }

        public void UpdateModel(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            UpdateElapsing();
            if (!IsTimeToElapse)
            {
                decimal timeBoundary = (decimal)timeSinceLastUpdate.TotalMilliseconds *15;

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
                    Physics.Position.X += totalForce.X / timeBoundary;
                    Physics.Position.Y += totalForce.Y / timeBoundary;
                }
                else
                {
                    // Calculate gravity.
                    Real2DVector gravity = CalculateGravity(otherThings);
                    totalForce.Add(gravity);

                    // Move the object.
                    Physics.Position.X += totalForce.X / Physics.Mass / timeBoundary;
                    Physics.Position.Y += totalForce.Y / Physics.Mass / timeBoundary;
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

                var x = thing.Physics.Position.X - Physics.Position.X;
                var y = thing.Physics.Position.Y - Physics.Position.Y;
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
            return Physics.Position.DistanceTo(another.Physics.Position);
        }

        public void Spawn(Point where, Real2DVector initialInertia = null)
        {
            Physics.SetPosition(where);
            Physics.Interia = initialInertia ?? new Real2DVector();
        }
    }
}
