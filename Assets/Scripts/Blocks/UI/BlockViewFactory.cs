using System.Collections.Generic;
using Blocks.UI.Skins;
using UnityEngine;
using Utilities.Pooling;

namespace Blocks.UI
{
    public class BlockViewFactory
    {
        private readonly BlockSkinLibrary m_SkinLibrary;

        private readonly Dictionary<BlockView, GameObjectPool<BlockView>> m_Pools = new();
        private readonly Dictionary<BlockView, GameObjectPool<BlockView>> m_InstanceToPool = new();
        
        public BlockViewFactory(BlockSkinLibrary skinLibrary)
        {
            m_SkinLibrary = skinLibrary;
        }

        private GameObjectPool<BlockView> GetOrCreatePool(BlockView blockViewPrefab, Transform parent = null)
        {
            if (m_Pools.TryGetValue(blockViewPrefab, out var existingPool))
            {
                return existingPool;
            }
            
            var pool = new GameObjectPool<BlockView>(blockViewPrefab, parent: parent);
            m_Pools.Add(blockViewPrefab, pool);
            
            return pool;
        }

        public BlockView SpawnView(Block block, Transform parent)
        {
            var skin = m_SkinLibrary.GetSkin(block.GetCategory(), block.GetTypeId());

            if (skin == null)
            {
                Debug.LogError($"No BlockSkin available for block with category {block.GetCategory()} and type {block.GetType()} with typeId {block.GetTypeId()}!");
                return null;
            }
            
            var prefab = skin.OverridePrefab ?? m_SkinLibrary.DefaultPrefab;

            if (prefab == null)
            {
                Debug.LogError("No BlockView prefab available! Provide a default in BlockSkinLibrary.");
                return null;
            }
            
            var pool = GetOrCreatePool(prefab, parent);
            var view = pool.Get(parent);
            
            view.Init(block, skin);
            m_InstanceToPool.Add(view, pool);
            
            return view;
        }

        public void ReleaseView(BlockView blockView)
        {
            if (!m_InstanceToPool.Remove(blockView, out var pool))
            {
                Debug.LogError("BlockView is not in any existing pools!");
                return;
            }

            pool.Release(blockView);
        }
    }
}