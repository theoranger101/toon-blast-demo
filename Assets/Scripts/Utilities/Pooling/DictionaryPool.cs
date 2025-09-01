using System.Collections.Generic;

namespace Utilities.Pooling
{
    public static class DictionaryPool<TKey, TValue>
    {
        private static readonly Queue<Dictionary<TKey, TValue>> queue = new(5);
        
        
        public static Dictionary<TKey, TValue> Get()
        {
            if (queue.Count < 1)
            {
                queue.Enqueue(new Dictionary<TKey, TValue>());
            }

            return queue.Dequeue();
        }

        public static void Release(Dictionary<TKey, TValue> dict)
        {
            dict.Clear();
            queue.Enqueue(dict);
        }
    }
}