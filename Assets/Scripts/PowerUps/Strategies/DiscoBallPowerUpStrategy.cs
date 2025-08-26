using Blocks;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;
using Utilities.Pooling;

namespace PowerUps.Strategies
{
    public class DiscoBallPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpBlock Owner { get; set; }
        
        public MatchBlockType TargetType { get; set; }
        
        public void Activate()
        {
            Debug.Log("Disco Ball PowerUp activated at position: " + Owner.GridPosition);

            var evt = GridEvent.Get(TargetType);
            evt.SendGlobal(channel: (int)GridEventType.RequestSameType);

            var blocksToPop = evt.Blocks;
            
            // TODO: if this is all repeated find a way to generalize it.
            for (var i = blocksToPop.Count - 1; i >= 0; i--)
            {
                if (blocksToPop[i] == Owner)
                {
                    continue;
                }

                if (blocksToPop[i] == null)
                {
                    blocksToPop.RemoveAt(i);
                    continue;
                }
                
                blocksToPop[i].Pop();
            }
            
            blocksToPop.Add(Owner);
            
            // TODO: Consider doing extensions for these events
            using (var refillEvent = GridEvent.Get(blocksToPop))
            {
                refillEvent.SendGlobal(channel: (int)GridEventType.TriggerRefill);
            }
            
            evt.Dispose();
            ListPool<Block>.Release(blocksToPop);
            
            Owner = null;
            
            // TODO: pooling for strategies
        }
    }
}