using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsConcurrentQueue
    {
        public static IEnumerable<TQueued> GetAll<TQueued>(this ConcurrentQueue<TQueued> queue)
        {
            TQueued currentItem;
            while (queue.TryDequeue(out currentItem))
            {
                yield return currentItem;
            }
        }
    }
}
