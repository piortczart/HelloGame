using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;

namespace HelloGame.Common.Model
{
    public abstract class ThingBase : ElapsingThing
    {
        public enum UpdateLocationSettings
        {
            All,
            ExcludeAngle
        }

        protected readonly ILogger Logger;
        public AlmostPhysics Physics { get; }
        public ThingBase Creator { get; }
        private bool CanBeMoved { get; }
        protected bool IsDestroyed { get; private set; }
        private static int _highestId;
        public int Id { get; }
        private readonly object _modelSynchronizer = new object();

        protected ThingBase(ILogger logger, ThingSettings settings, ThingBase creator = null, int? id = null) : base(settings.TimeToLive)
        {
            Logger = logger;
            Physics = new AlmostPhysics(settings.Aerodynamism);
            Creator = creator;
            Physics.Mass = settings.Mass;
            Physics.RadPerSecond = settings.RadPerSecond;
            CanBeMoved = settings.CanBeMoved;
            ElapseIn(settings.TimeToLive);
            Id = id ?? Interlocked.Add(ref _highestId, 1);
        }

        protected abstract void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings);
        public abstract void CollidesWith(ThingBase other);
        public abstract void PaintStuff(Graphics g);

        protected void Destroy(TimeSpan elapseIn)
        {
            IsDestroyed = true;
            ElapseIn(elapseIn);
        }

        public void UpdateModel(TimeSpan timeSinceLastUpdate, List<ThingBase> otherThings)
        {
            lock (_modelSynchronizer)
            {
                UpdateElapsing();

                if (!IsTimeToElapse)
                {
                    // Update stuff like propelling.
                    UpdateModelInternal(timeSinceLastUpdate, otherThings);

                    if (CanBeMoved)
                    {
                        // Add the propeller force to the interia.
                        Physics.Interia.Add(Physics.SelfPropelling);

                        // Drag changes the inertia?
                        Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism * 0.05m);
                        Physics.Interia.Add(Physics.Drag);

                        Real2DVector totalForce = Physics.TotalForce;

                        // No mass? No gravity. Think lazer.
                        decimal timeBoundary = (decimal)(timeSinceLastUpdate.TotalMilliseconds / 100);
                        if (Physics.Mass == 0)
                        {
                            // No mass? 
                            Physics.Position.X += totalForce.X * timeBoundary;
                            Physics.Position.Y += totalForce.Y * timeBoundary;
                        }
                        else
                        {
                            // Calculate gravity.
                            Physics.Gravity = CalculateGravity(otherThings);
                            totalForce.Add(Physics.Gravity);

                            // Move the object.
                            Physics.Position.X += totalForce.X / Physics.Mass * timeBoundary;
                            Physics.Position.Y += totalForce.Y / Physics.Mass * timeBoundary;
                        }

                        // Too far away! DIE!
                        if (Math.Abs(Physics.Position.X) > 100000 || Math.Abs(Physics.Position.Y) > 100000)
                        {
                            Despawn();
                        }
                    }
                }
            }
        }

        private Real2DVector CalculateGravity(IEnumerable<ThingBase> otherThings)
        {
            Real2DVector result = new Real2DVector();

            foreach (ThingBase thing in otherThings)
            {
                if (thing == this) { continue; }
                var distance = thing.DistanceTo(this);
                if (distance == 0) { continue; }

                decimal length = 0.01m * Physics.Mass * thing.Physics.Mass / (decimal)Math.Pow((double)distance, 2);
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

        public decimal DistanceTo(ThingBase another)
        {
            return Physics.Position.DistanceTo(another.Physics.Position) - ((Physics.Size + another.Physics.Size) / 2);
        }

        public void Spawn(Point where, Real2DVector initialInertia = null)
        {
            Physics.SetPosition(where);
            Physics.Interia = initialInertia ?? new Real2DVector();
        }

        public void UpdateLocation(ThingBase otherThing, UpdateLocationSettings settings = UpdateLocationSettings.All)
        {
            lock (_modelSynchronizer)
            {
                decimal positionShift = Physics.Position.DistanceTo(otherThing.Physics.Position);
                Logger.LogInfo($"Total thing position shift: {positionShift}");

                if (positionShift > 20)
                {
                    throw new Exception($"Position shift too high! {positionShift}");
                }

                Physics.Update(otherThing.Physics, settings);

                if (this is AiShip)
                {
                    Logger.LogInfo($"Updated angle to: {Physics.Angle}");
                }

                IsDestroyed = otherThing.IsDestroyed;
                ElapseIn(otherThing.TimeToLive);
            }
        }
    }
}
