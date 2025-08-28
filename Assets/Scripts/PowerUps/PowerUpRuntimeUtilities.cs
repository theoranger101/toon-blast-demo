using System.Collections.Generic;
using Blocks;
using Blocks.BlockTypes;
using UnityEngine;

namespace PowerUps
{
    internal static class PowerUpRuntimeUtilities
    {
        public static void PopTargetsThenOwner(this PowerUpBlock owner, IList<Block> targets, bool popOwner = true)
        {
            if (targets == null)
            {
                Debug.LogWarning($"Target list unavailable for PowerUp type {owner.Type} at position {owner.GridPosition}");
                return;
            }

            for (var i = 0; i < targets.Count; i++)
            {
                var block = targets[i];
                if (block == null || ReferenceEquals(block, owner))
                {
                    continue;
                }
                
                block.Pop();
            }

            if (!popOwner || owner == null)
            {
                return;
            }
            
            owner.Pop();
        }
    }
}