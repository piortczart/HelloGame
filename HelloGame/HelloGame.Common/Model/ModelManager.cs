using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace HelloGame.Common.Model
{
    public class ModelManager
    {
        private readonly Action _updateModelAction;
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        readonly CollisionDetector _collidor = new CollisionDetector();
        private TimeSpan _lastModelUpdate = TimeSpan.Zero;
        private readonly EventPerSecond _modelUpdateCounter = new EventPerSecond();
        private readonly Thread _modelUpdateThread;
        private readonly SynchronizedCollection<ThingBase> _things = new SynchronizedCollection<ThingBase>();

        public ModelManager(Action updateModelAction = null)
        {
            _updateModelAction = updateModelAction;
            _modelUpdateThread = new Thread(UpdateModel) { IsBackground = true };
        }

        public void AddThing(ThingBase thingBase)
        {
            _things.Add(thingBase);
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

                    var nonModifiable = new List<ThingBase>(_things);
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