using System.Collections.Concurrent;
using System.Collections.Generic;

namespace HelloGame.Common.Extensions
{
    public static class ExtensionsConcurrentQueue
    {
        public static List<TQueued> GetAll<TQueued>(this ConcurrentQueue<TQueued> queue)
        {
            var result = new List<TQueued>();
            TQueued currentItem;
            while (queue.TryDequeue(out currentItem))
            {
                result.Add(currentItem);
            }
            return result;
        }
    }
}
