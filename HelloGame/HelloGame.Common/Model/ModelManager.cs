using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Logging;
using System.Collections.Concurrent;
using HelloGame.Common.Extensions;

namespace HelloGame.Common.Model
{
    public class ModelManager
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        readonly CollisionDetector _collidor = new CollisionDetector();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        private readonly EventPerSecond _modelUpdateCounter = new EventPerSecond();
        private readonly Thread _modelUpdateThread;
        private readonly ThreadSafeList<ThingBase> _things = new ThreadSafeList<ThingBase>();
        private readonly ConcurrentQueue<ThingBase> _deadThings = new ConcurrentQueue<ThingBase>();
        private readonly List<Action> _updateModelAction = new List<Action>();
        public EventPerSecond CollisionCalculations { get { return _collidor.CollisoinsCounter; } }
        Overlay _overlay;

        public ModelManager(ILoggerFactory loggerFactory, Overlay overlay)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _modelUpdateThread = new Thread(UpdateModel) { IsBackground = true };
            _overlay = overlay;
        }

        public void AddUpdateModelAction(Action action)
        {
            _updateModelAction.Add(action);
        }

        public void UpdateThing(ThingBase thingBase)
        {
            ThingBase existing = _things.SingleOrDefault(t => t.Id == thingBase.Id);
            if (existing == null)
            {
                _things.Add(thingBase);
            }
            else
            {
                existing.UpdateLocation(thingBase);
            }
        }

        public List<ThingBase> GetThings()
        {
            return _things.ToList();
        }

        /// <summary>
        /// Returns all things that have been despawned since last call of this method.
        /// </summary>
        /// <returns></returns>
        public List<ThingBase> GetDeadThings()
        {
            return _deadThings.GetAll();
        }

        public void StartModelUpdates()
        {
            _modelUpdateThread.Start();
        }

        private void UpdateModel()
        {
            while (_modelUpdateThread.IsAlive)
            {
                _overlay.Update(this);

                _modelUpdateCounter.Add();

                TimeSpan now = _stopwatch.Elapsed;
                if (_lastModelUpdate != TimeSpan.Zero)
                {
                    TimeSpan sinceLast = now - _lastModelUpdate;

                    List<ThingBase> nonModifiable = _things.ToList().OrderBy(t => t.Id).ToList();
                    Parallel.ForEach(nonModifiable, item =>
                    {
                        item.UpdateModel(sinceLast, nonModifiable);
                        if (item.IsTimeToElapse)
                        {
                            _things.Remove(item);
                            _deadThings.Enqueue(item);
                        }
                    });
                    _collidor.DetectCollisions(_things.ToList().Where(t => t != null).ToList());
                }
                _lastModelUpdate = now;

                foreach (var action in _updateModelAction)
                {
                    action.Invoke();
                }
            }
        }
    }
}