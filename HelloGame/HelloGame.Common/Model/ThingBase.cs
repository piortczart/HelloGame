using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using HelloGame.Common.Logging;
using HelloGame.Common.MathStuff;
using HelloGame.Common.Physicsish;
using System.Windows.Forms;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model.GameObjects.Ships;
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
        public bool IsDestroyed { get; private set; }
        private static int _highestId;
        public int Id { get; }
        public int Deaths { get; set; }
        private readonly object _modelSynchronizer = new object();
        public Shield Shield { get; private set; }
        public Weapons Weapons { get; private set; }
        protected readonly Font Font = new Font("monospace", 12, GraphicsUnit.Pixel);
        public readonly ThingSettings Settingz;
        protected readonly ThingBaseInjections Injections;
        protected readonly GeneralSettings Settings;
        protected readonly GameThingCoordinator Coordinator;

        public virtual ThingAdditionalInfo ThingAdditionalInfo
            =>
                new ThingAdditionalInfo
                {
                    IsDestroyed = IsDestroyed,
                    CreatorId = Creator?.Id,
                    WeaponsSerialized = Weapons.SerializeJson(),
                    ShieldSerialized = Shield.SerializeJson()
                };

        protected ThingBase(ThingBaseInjections injections, ThingSettings settings, ThingAdditionalInfo additionalInfo,
            int? id = null)
            : base(settings.TimeToLive, injections.TimeSource, settings.SpawnedAt)
        {
            // First time spawned? Weapons needs to be initially created.
            // If it already exists, the weapons should come from the ThingAdditionalInfo
            Weapons = id == null ? settings.InitialWeapons : additionalInfo.GetWeapons();
            Shield = id == null ? settings.InitialShield : additionalInfo.GetShield();

            Coordinator = injections.Coordinator;
            Settings = injections.GeneralSettings;
            Injections = injections;
            Settingz = settings;
            Logger = injections.LoggerFactory.CreateLogger(GetType());
            Physics = new AlmostPhysics(settings.Aerodynamism);
            Creator = additionalInfo.Creator;
            Physics.Mass = settings.Mass;
            Physics.RadPerSecond = settings.RadPerSecond;
            ElapseIn(settings.TimeToLive);
            Id = id ?? Interlocked.Add(ref _highestId, 1);
            IsDestroyed = additionalInfo.IsDestroyed;
        }

        /// <summary>
        /// Be aware, this is called by the ModelUpdate thread.
        /// </summary>
        protected abstract void UpdateModelInternal(TimeSpan timeSinceLastUpdate, IEnumerable<ThingBase> otherThings);

        /// <summary>
        /// Be aware, this is called by the GUI thread (WinForms Timer)
        /// </summary>
        protected abstract void Render(Graphics g);

        protected abstract void CollidesWithInternal(ThingBase other);

        public void CollidesWith(ThingBase other)
        {
            CollidesWithInternal(other);

            // If the owner is a ship and the "other" thing was destoryed, we score points!
            var shipBase = Creator as ShipBase;
            if (shipBase != null && other.IsDestroyed)
            {
                shipBase.Score += Settingz.PointsForKilling;
            }
        }

        protected bool DealDamage(decimal damage, TimeSpan? despawnTime = null, ThingBase source = null)
        {
            bool shieldsDown = Shield == null || Shield.DamageDealt(damage);
            if (shieldsDown) {
                Destroy(despawnTime ?? TimeSpan.Zero, source);
            }
            return shieldsDown;
        }

        public void RenderBase(Graphics g)
        {
            Render(g);

            // Draw the shield if it's there.
            if (Shield != null)
            {
                var colorRgb = (int)((255-(int)Math.Min(255 * Shield.Percentage / 100, 255)) * 0.5) + 127;
                var color = Color.FromArgb(colorRgb, colorRgb, colorRgb);
                var radius = (int)(Physics.Size/2 + 10);
                g.DrawCircle(Physics.PositionPoint, radius, color);
            }

            // Show the creator's id below this thing
            if (Settings.ShowThingIds)
            {
                string owner = $"{Id} ({Creator?.Id.ToString() ?? "?"}) SH: {Shield?.Percentage}";
                Size ownerSize = TextRenderer.MeasureText(owner, Font);
                var nameLocation = new PointF((int)Physics.Position.X - ownerSize.Width / 2,
                    (int)Physics.Position.Y + (int)Settingz.Size + ownerSize.Height * 4);
                g.DrawString(owner, Font, Brushes.Black, nameLocation);
            }

            if (Settings.ShowTimeToLive)
            {
                string text = TimeToLive.ToString();
                Size textSize = TextRenderer.MeasureText(text, Font);

                var nameLocation = new PointF((int)Physics.Position.X - textSize.Width / 2,
                    (int)Physics.Position.Y + (int)Settingz.Size + textSize.Height * 2);
                g.DrawString(text, Font, Brushes.Black, nameLocation);
            }
        }

        /// <summary>
        /// Destroys this thing.
        /// </summary>
        /// <param name="elapseIn">The amount of time to elapse in.</param>
        protected void Destroy(TimeSpan elapseIn, ThingBase destroyer)
        {
            lock (_modelSynchronizer)
            {
                IsDestroyed = true;
                Deaths++;
                ElapseIn(elapseIn);
            }
        }

        public void UpdateModel(TimeSpan timeSinceLastUpdate, IReadOnlyCollection<ThingBase> otherThings)
        {
            lock (_modelSynchronizer)
            {
                UpdateElapsing();

                if (!IsTimeToElapse && !IsDestroyed)
                {
                    // Update stuff like propelling.
                    UpdateModelInternal(timeSinceLastUpdate, otherThings);

                    UpdatePhysics(timeSinceLastUpdate, otherThings);

                    // Too far away! DIE!
                    if (
                        Physics.Position.X > Settings.GameSize.Width ||
                        Physics.Position.X < 0 ||
                        Physics.Position.Y > Settings.GameSize.Height ||
                        Physics.Position.Y < 0)
                    {
                        Despawn();
                    }
                }
            }
        }

        private void UpdatePhysics(TimeSpan timeSinceLastUpdate, IReadOnlyCollection<ThingBase> otherThings)
        {
            if (Settingz.CanBeMoved)
            {
                decimal timeBoundary = (decimal)(timeSinceLastUpdate.TotalMilliseconds / 100);

                // Add the propeller force to the interia.
                Physics.Interia.Add(Physics.SelfPropelling.GetScaled(timeBoundary));

                // Drag changes the inertia?
                Physics.Drag = Physics.Interia.GetOpposite().GetScaled(Physics.Aerodynamism * 0.5m * timeBoundary);
                Physics.Interia.Add(Physics.Drag);

                Vector2D totalForce = Physics.TotalForce;

                // No mass? No gravity. Think lazer.
                // AiShips have antigravity.
                if (Physics.Mass == 0)
                {
                    // No mass? 
                    Physics.PositionDelta(totalForce.X * timeBoundary, totalForce.Y * timeBoundary);
                }
                else
                {
                    // Calculate gravity.
                    if (!Settingz.Antigravity)
                    {
                        Physics.Gravity = CalculateGravity(otherThings);
                        totalForce.Add(Physics.Gravity);
                    }

                    // Move the object.
                    decimal deltaX = totalForce.X / Physics.Mass * timeBoundary;
                    decimal deltaY = totalForce.Y / Physics.Mass * timeBoundary;
                    Physics.PositionDelta(deltaX, deltaY);
                }
            }
        }

        private Vector2D CalculateGravity(IEnumerable<ThingBase> otherThings)
        {
            Vector2D result = new Vector2D();

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

                decimal length = Settings.GravityFactor * Physics.Mass * thing.Physics.Mass /
                                 (decimal)Math.Pow((double)distance, 2);
                var grav = new Vector2D();

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
                return Physics.Position.DistanceTo(another.Physics.Position) - ((Physics.Size + another.Physics.Size) / 2);
            }
        }

        public void Spawn(Point where, Vector2D initialInertia = null)
        {
            lock (_modelSynchronizer)
            {
                Physics.SetInitialPosition(where);
                Physics.Interia = initialInertia ?? new Vector2D();
            }
        }

        public void UpdateState(ThingBase otherThing, UpdateLocationSettings settings)
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