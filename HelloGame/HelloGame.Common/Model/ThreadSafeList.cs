using System.Collections.Generic;

namespace HelloGame.Common.Model
{
    /// <summary>
    /// Poor man's thread-safe list implementaion.
    /// </summary>
    public class ThreadSafeList<T> : List<T>
    {
        private readonly List<T> _interalList = new List<T>();
        private static readonly object _lock = new object();
        
        public new IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }


        public List<T> Clone()
        {
            List<T> newList = new List<T>();

            lock (_lock)
            {
                _interalList.ForEach(x => newList.Add(x));
            }

            return newList;
        }

    }
}