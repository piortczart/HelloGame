using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Model.GameObjects.Ships;
using HelloGame.Common.Physicsish;
using System.Windows.Forms;
using HelloGame.Common.Settings;

namespace HelloGame.Common.Model
{
    public abstract class ThingBase : ElapsingThing
    {
        public enum UpdateLocationSettings
        {
            All,
            ExcludePositionAndAngle,
            AngleAndEngineAndPosition
        }

        protected readonly ILogger Logger;
        public AlmostPhysics Physics { get; }
        public ThingBase Creator { get; }
        private bool CanBeMoved { get; }
        public bool IsDestroyed { get; private set; }
        private static int _highestId;
        public int Id { get; }
        private readonly object _modelSynchronizer = new object();
        protected readonly Font Font = new Font("monospace", 12, GraphicsUnit.Pixel);
        public readonly ThingSettings Settingz;
        protected readonly ThingBaseInjections Injections;
        protected readonly GeneralSettings Settings;
        protected readonly GameThingCoordinator Coordinator;

        protected ThingBase(ThingBaseInjections injections, ThingSettings settings, ThingBase creator = null,
            int? id = null)
            : base(settings.TimeToLive, injections.TimeSource, settings.SpawnedAt)
        {
            Coordinator = injections.Coordinator;
            Settings = injections.GeneralSettings;
            Injections = injections;
            Settingz = settings;
            Logger = injections.LoggerFactory.CreateLogger(GetType());
            Physics = new AlmostPhysics(settings.Aerodynamism);
            Creator = creator;
            Physics.Mass = settings.Mass;
            Physics.RadPerSecond = settings.RadPerSecond;
            CanBeMoved = settings.CanBeMoved;
            ElapseIn(settings.TimeToLive);
            Id = id ?? Interlocked.Add(ref _highestId, 1);
        }

        /// <summary>
        /// Be aware, this is called by the ModelUpdate thread.
        /// </summary>
        protected abstract void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings);

        /// <summary>
        /// Be aware, this is called by the GUI thread (WinForms Timer)
        /// </summary>
        protected abstract void Render(Graphics g);

        public abstract void CollidesWith(ThingBase other);

        public void RenderBase(Graphics g)
        {
            Render(g);

            // Show the creator's id below this thing
            if (Settings.ShowThingIds)
            {
                string owner = $"{Id} ({Creator?.Id.ToString() ?? "?"})";
                Size ownerSize = TextRenderer.MeasureText(owner, Font);
                var nameLocation = new PointF((int) Physics.Position.X - ownerSize.Width/2,
                    (int) Physics.Position.Y + (int) Settingz.Size + ownerSize.Height*4);
                g.DrawString(owner, Font, Brushes.Black, nameLocation);
            }

            if (Settings.ShowTimeToLive)
            {
                string text = TimeToLive.ToString();
                Size textSize = TextRenderer.MeasureText(text, Font);

                var nameLocation = new PointF((int) Physics.Position.X - textSize.Width/2,
                    (int) Physics.Position.Y + (int) Settingz.Size + textSize.Height*2);
                g.DrawString(text, Font, Brushes.Black, nameLocation);
            }
        }

        protected void Destroy(TimeSpan elapseIn)
        {
            lock (_modelSynchronizer)
            {
                IsDestroyed = true;
                ElapseIn(elapseIn);
            }
        }

        public void UpdateModel(TimeSpan timeSinceLastUpdate, IReadOnlyCollection<ThingBase> otherThings)
        {
            lock (_modelSynchronizer)
            {
                UpdateElapsing();

                if (!IsTimeToElapse)
                {
                    // Update stuff like propelling.
                    UpdateModelInternal(timeSinceLastUpdate, otherThings);

                    UpdatePhysics(timeSinceLastUpdate, otherThings);

                    // Too far away! DIE!
                    if (Math.Abs(Physics.Position.X) > 10000 || Math.Abs(Physics.Position.Y) > 10000)
                    {
                        Despawn();
                    }
                }
            }
        }

        private void UpdatePhysics(TimeSpan timeSinceLastUpdate, IReadOnlyCollection<ThingBase> otherThings)
        {
            if (CanBeMoved)
            {
                decimal timeBoundary = (decimal) (timeSinceLastUpdate.TotalMilliseconds/100);

                // Add the propeller force to the interia.
                Physics.Interia.Add(Physics.SelfPropelling.GetScaled(timeBoundary));

                // Drag changes the inertia?
                Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism*0.5m*timeBoundary);
                Physics.Interia.Add(Physics.Drag);

                Real2DVector totalForce = Physics.TotalForce;

                // No mass? No gravity. Think lazer.
                if (Physics.Mass == 0)
                {
                    // No mass? 
                    Physics.PositionDelta(totalForce.X*timeBoundary, totalForce.Y*timeBoundary);
                }
                else
                {
                    // Calculate gravity.
                    Physics.Gravity = CalculateGravity(otherThings);
                    totalForce.Add(Physics.Gravity);

                    // Move the object.
                    decimal newX = totalForce.X/Physics.Mass*timeBoundary;
                    decimal newY = totalForce.Y/Physics.Mass*timeBoundary;
                    Physics.PositionDelta(newX, newY);
                }
            }
        }

        private Real2DVector CalculateGravity(IEnumerable<ThingBase> otherThings)
        {
            Real2DVector result = new Real2DVector();

            foreach (ThingBase thing in otherThings)
            {
                if (thing == this)
                {
                    continue;
                }
                var distance = thing.DistanceTo(this);
                if (distance == 0)
                {
                    continue;
                }

                decimal length = Settings.GravityFactor*Physics.Mass*thing.Physics.Mass/
                                 (decimal) Math.Pow((double) distance, 2);
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

        public decimal DistanceTo(Position position)
        {
            return position.DistanceTo(Physics.Position) - Physics.Size;
        }

        public decimal DistanceTo(ThingBase another)
        {
            lock (_modelSynchronizer)
            {
                return Physics.Position.DistanceTo(another.Physics.Position) - ((Physics.Size + another.Physics.Size)/2);
            }
        }

        public void Spawn(Point where, Real2DVector initialInertia = null)
        {
            lock (_modelSynchronizer)
            {
                Physics.SetInitialPosition(where);
                Physics.Interia = initialInertia ?? new Real2DVector();
            }
        }

        public void UpdateLocation(ThingBase otherThing, UpdateLocationSettings settings)
        {
            lock (_modelSynchronizer)
            {
                // Update this object's physics.
                Physics.Update(otherThing.Physics, settings);

                switch (settings)
                {
                    case UpdateLocationSettings.All:
                        IsDestroyed = otherThing.IsDestroyed;
                        ElapseIn(otherThing.TimeToLive);
                        break;
                    case UpdateLocationSettings.ExcludePositionAndAngle:
                        IsDestroyed = otherThing.IsDestroyed;
                        ElapseIn(otherThing.TimeToLive);
                        break;
                    case UpdateLocationSettings.AngleAndEngineAndPosition:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(settings), settings, null);
                }
            }
        }
    }
}