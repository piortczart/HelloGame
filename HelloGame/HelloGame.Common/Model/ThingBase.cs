using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using System.Windows.Forms;

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
        protected readonly Font Font = new Font("monospace", 12, GraphicsUnit.Pixel);
        protected readonly ThingSettings Settingz;

        protected ThingBase(ILogger logger, ThingSettings settings, ThingBase creator = null, int? id = null) : base(settings.TimeToLive)
        {
            Settingz = settings;
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
        public abstract void Render(Graphics g);

        public void RenderBase(Graphics g)
        {
            Render(g);

            string owner = Creator?.Id.ToString() ?? "?";
            Size nameSize = TextRenderer.MeasureText(owner, Font);
            var nameLocation = new PointF((int)Physics.Position.X - nameSize.Width / 2, (int)Physics.Position.Y + (int)Settingz.Size + nameSize.Height * 2);
            g.DrawString(owner, Font, Brushes.Black, nameLocation);
        }

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
                        var before = new Position(Physics.Position.X, Physics.Position.Y);

                        decimal timeBoundary = (decimal)(timeSinceLastUpdate.TotalMilliseconds / 100);

                        // Add the propeller force to the interia.
                        Physics.Interia.Add(Physics.SelfPropelling.GetScaled(timeBoundary));

                        // Drag changes the inertia?
                        Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism * 0.5m * timeBoundary);
                        Physics.Interia.Add(Physics.Drag);

                        Real2DVector totalForce = Physics.TotalForce;

                        // No mass? No gravity. Think lazer.
                        if (Physics.Mass == 0)
                        {
                            // No mass? 
                            Physics.PositionDelta(totalForce.X * timeBoundary, totalForce.Y * timeBoundary);
                        }
                        else
                        {
                            // Calculate gravity.
                            Physics.Gravity = CalculateGravity(otherThings);
                            totalForce.Add(Physics.Gravity);

                            // Move the object.
                            decimal newX = totalForce.X / Physics.Mass * timeBoundary;
                            decimal newY = totalForce.Y / Physics.Mass * timeBoundary;
                            Physics.PositionDelta(newX, newY);
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
            Physics.SetInitialPosition(where);
            Physics.Interia = initialInertia ?? new Real2DVector();
        }

        public void UpdateLocation(ThingBase otherThing)
        {
            lock (_modelSynchronizer)
            {
                decimal positionShift = Physics.Position.DistanceTo(otherThing.Physics.Position);
                //Logger.LogInfo($"Total thing position shift: {positionShift}");

                if (positionShift > 20)
                {
                    //throw new Exception($"Position shift too high! {positionShift}");
                }

                var settings = this is PlayerShipMovable ? UpdateLocationSettings.ExcludeAngle : UpdateLocationSettings.All;
                Physics.Update(otherThing.Physics, settings);

                IsDestroyed = otherThing.IsDestroyed;
                ElapseIn(otherThing.TimeToLive);
            }
        }
    }
}
