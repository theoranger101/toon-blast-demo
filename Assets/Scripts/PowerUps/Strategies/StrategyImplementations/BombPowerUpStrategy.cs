using Blocks;
using Blocks.BlockTypes;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace PowerUps.Strategies
{
    public sealed class BombPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpType PowerUpType => PowerUpType.Bomb;
        
        public PowerUpBlock Owner { get; set; }
        
        public void Plan(PowerUpResolver resolver)
        {
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Bomb PowerUp activated at position: {Owner.GridPosition}");

            var evt = GridEvent.Get(Owner.GridPosition);
            evt.SendGlobal(channel: (int)GridEventType.RequestAdjacent);
            
            resolver.EnqueueMany(evt.Blocks, Owner);
            
            evt.Dispose();
        }

        public void Reset()
        {
            Owner = null;
        }
    }
}