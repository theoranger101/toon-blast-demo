using Blocks;
using Blocks.BlockTypes;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace PowerUps.Strategies
{
    public sealed class DiscoBallPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpType PowerUpType => PowerUpType.DiscoBall;
        
        public PowerUpBlock Owner { get; set; }
        
        public MatchBlockType TargetType { get; set; }
        
        public void Plan(PowerUpResolver resolver)
        {
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Disco Ball PowerUp targeting {TargetType} blocks activated at position: {Owner.GridPosition}");

            var evt = GridEvent.Get(TargetType);
            evt.SendGlobal(channel: (int)GridEventType.RequestSameType);

            resolver.EnqueueMany(evt.Blocks, Owner);
            
            evt.Dispose();
        }

        public void Reset()
        {
            Owner = null;
            TargetType = default;
        }
    }
}