using System.Collections.Generic;

namespace Utilities.Pooling
{
    public static class HashSetPool<T>
    {
        private static readonly Queue<HashSet<T>> queue = new(5);

        public static HashSet<T> Get()
        {
            if (queue.Count < 1)
            {
                queue.Enqueue(new HashSet<T>());
            }

            return queue.Dequeue();
        }

        public static void Release(HashSet<T> list)
        {
            list.Clear();
            queue.Enqueue(list);
        }
    }
}