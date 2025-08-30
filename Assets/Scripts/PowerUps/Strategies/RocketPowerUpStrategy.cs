using Blocks.BlockTypes;
using Grid;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace PowerUps.Strategies
{
    public class RocketPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpBlock Owner { get; set; }
        public GridAxis Orientation { get; set; }

        public void Activate()
        {
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Rocket PowerUp with Orientation {Orientation} activated at position {Owner.GridPosition}");

            var gridPosition = Owner.GridPosition;
            var evt = GridEvent.Get(Orientation, gridPosition);
            evt.SendGlobal(channel: (int)GridEventType.RequestAxis);

            Owner.PopTargetsThenOwner(evt.Blocks, popOwner: false);
            
            evt.Dispose();
            Owner = null;
            
            // TODO: pooling for strategies
        }
    }
}