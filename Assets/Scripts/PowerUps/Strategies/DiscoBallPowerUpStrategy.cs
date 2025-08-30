using Blocks;
using Blocks.BlockTypes;
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
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Disco Ball PowerUp targeting {TargetType} blocks activated at position: {Owner.GridPosition}");

            var evt = GridEvent.Get(TargetType);
            evt.SendGlobal(channel: (int)GridEventType.RequestSameType);

            Owner.PopTargetsThenOwner(evt.Blocks, popOwner: false);
            
            evt.Dispose();
            Owner = null;
            
            // TODO: pooling for strategies
        }
    }
}