using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelloGame.Common.Logging;

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
        private readonly SynchronizedCollection<ThingBase> _things = new SynchronizedCollection<ThingBase>();
        private Action _updateModelAction;

        public ModelManager(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _modelUpdateThread = new Thread(UpdateModel) { IsBackground = true };
        }

        public void SetUpdateModelAction(Action action)
        {
            if (_updateModelAction != null)
            {
                throw new Exception("There is a model action already attached.");
            }
            _updateModelAction = action;
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
            return new List<ThingBase>(_things);
        }

        public void StartModelUpdates()
        {
            _modelUpdateThread.Start();
        }

        private void UpdateModel()
        {
            while (_modelUpdateThread.IsAlive)
            {
                _modelUpdateCounter.Add();

                TimeSpan now = _stopwatch.Elapsed;
                if (_lastModelUpdate != TimeSpan.Zero)
                {
                    TimeSpan sinceLast = now - _lastModelUpdate;

                    List<ThingBase> nonModifiable = _things.OrderBy(t=>t.Id).ToList();
                    Parallel.ForEach(nonModifiable, item =>
                    {
                        item.UpdateModel(sinceLast, nonModifiable);
                        if (item.IsTimeToElapse)
                        {
                            if (_things.Contains(item))
                            {
                                _things.Remove(item);
                            }
                        }
                    });
                    _collidor.DetectCollisions(_things);
                }
                _lastModelUpdate = now;

                _updateModelAction?.Invoke();
            }
        }
    }
}