using System;
using System.Collections.Generic;
using Blocks;
using Blocks.BlockTypes;
using Grid;
using UnityEngine;
using Utilities.Pooling;

namespace PowerUps
{
    public sealed class PowerUpResolver : IDisposable
    {
        private readonly GridManager m_Grid;
        private GridManager.GridRefillResolutionScope m_RefillResolutionScope;

        private readonly Queue<Block> m_PlannedQueue;
        private readonly HashSet<Block> m_SeenBlocks;
        private readonly List<Block> m_ToPop;

        public PowerUpResolver(GridManager grid)
        {
            m_Grid = grid;
            m_RefillResolutionScope = grid.ResolutionBatch;
            
            m_PlannedQueue = QueuePool<Block>.Get();
            m_SeenBlocks = HashSetPool<Block>.Get();
            m_ToPop = ListPool<Block>.Get();
        }

        public void Dispose()
        {
            m_RefillResolutionScope.Dispose();
            
            QueuePool<Block>.Release(m_PlannedQueue);
            HashSetPool<Block>.Release(m_SeenBlocks);
            ListPool<Block>.Release(m_ToPop);
        }

        public void Enqueue(Block block)
        {
            if (block == null)
            {
                return;
            }

            if (m_SeenBlocks.Add(block))
            {
                m_PlannedQueue.Enqueue(block);
            }
        }

        /// <summary>
        /// To be used by activated PowerUps.
        /// </summary>
        /// <param name="blocks"> Blocks targeted by the PowerUp </param>
        /// <param name="owner"> PowerUp activated </param>
        public void EnqueueMany(IList<Block> blocks, Block owner = null)
        {
            if (blocks == null)
            {
                return;
            }

            for (var i = 0; i < blocks.Count; i++)
            {
                var block = blocks[i];
                if (block == null)
                {
                    continue;
                }
                if (owner != null && ReferenceEquals(owner, block))
                {
                    continue; // avoid targeting the owner block twice.
                }
                
                Enqueue(block);
            }
        }

        public void SeedFromClick(Block clicked)
        {
            if (clicked is PowerUpBlock powerUpBlock)
            {
                powerUpBlock.Strategy?.Plan(this);
            }

            Enqueue(clicked);
        }

        public void PlanAll()
        {
            while (m_PlannedQueue.Count > 0)
            {
                var block = m_PlannedQueue.Dequeue();

                if (block == null || block.IsPopped)
                {
                    continue;
                }

                if (block is PowerUpBlock powerUpBlock)
                {
                    powerUpBlock.Strategy?.Plan(this);
                }
                
                m_ToPop.Add(block);
            }
        }

        public void Apply()
        {
            for (var i = 0; i < m_ToPop.Count; i++)
            {
                var block = m_ToPop[i];
                if (block == null || block.IsPopped)
                {
                    Debug.LogWarning("Tried to pop invalid or already popped block!");
                    continue;
                }
                
                block.Pop();
            }
        }
    }
}