using System.Collections.Generic;

namespace Utilities.Pooling
{
    public static class ListPool<T>
    {
        private static readonly Queue<List<T>> queue = new(5);

        public static List<T> Get()
        {
            if (queue.Count < 1)
            {
                queue.Enqueue(new List<T>());
            }

            return queue.Dequeue();
        }

        public static void Release(List<T> list)
        {
            list.Clear();
            queue.Enqueue(list);
        }
    }
}