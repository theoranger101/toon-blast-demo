using System.Collections.Generic;
using UnityEngine;
using Utilities.Pooling;

namespace Blocks.UI
{
    public class BlockViewFactory
    {
        private Dictionary<MonoBehaviour, object> m_BlockViewPools = new();

        public GameObjectPool<T> GetOrCreatePool<T>(T prefab) where T : MonoBehaviour
        {
            if (m_BlockViewPools.TryGetValue(prefab, out var existing))
            {
                return (GameObjectPool<T>)existing;
            }
            
            var pool = new GameObjectPool<T>(prefab);
            m_BlockViewPools[prefab] = pool;
            return pool;
        }
        
        public BlockView SpawnView(Block block)
        {
            BlockView view;

            switch (block)
            {
                case MatchBlock
            }
        }
    }
}