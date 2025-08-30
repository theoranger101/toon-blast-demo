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
            
            evt.Dispose();
            Owner = null;
            // TODO: pooling for strategies
        }
    }
}