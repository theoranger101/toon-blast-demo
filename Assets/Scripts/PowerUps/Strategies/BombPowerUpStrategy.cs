using Blocks.BlockTypes;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace PowerUps.Strategies
{
    public class BombPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpBlock Owner { get; set; }
        
        public void Activate()
        {
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Bomb PowerUp activated at position: {Owner.GridPosition}");

            var evt = GridEvent.Get(Owner.GridPosition);
            evt.SendGlobal(channel: (int)GridEventType.RequestAdjacent);

            Owner.PopTargetsThenOwner(evt.Blocks, popOwner: false);
            
            /*
            var blocksToPop = evt.Blocks;
            var cellsToEmpty = HashSetPool<Vector2Int>.Get();
            
            // TODO: if this is all repeated find a way to generalize it.
            for (var i = blocksToPop.Count - 1; i >= 0; i--)
            {
                var block = blocksToPop[i];
                
                if (block == null)
                {
                    continue;
                }
                
                if (block == Owner)
                {
                    continue;
                }
                
                block.Pop();
                cellsToEmpty.Add(block.GridPosition);
            }
            
            cellsToEmpty.Add(Owner.GridPosition);
            
            // TODO: Consider doing extensions for these events
            using (var refillEvent = GridEvent.Get(cellsToEmpty))
            {
                refillEvent.SendGlobal(channel: (int)GridEventType.TriggerRefill);
            }
            */
            
            evt.Dispose();
            Owner = null;
            
            /*
            ListPool<Block>.Release(blocksToPop);
            HashSetPool<Vector2Int>.Release(cellsToEmpty);
            */
            // TODO: pooling for strategies
        }
    }
}