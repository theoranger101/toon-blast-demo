using Blocks;
using Blocks.BlockTypes;
using Grid;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;

namespace PowerUps.Strategies
{
    public sealed class RocketPowerUpStrategy : IPowerUpStrategy
    {
        public PowerUpType PowerUpType => PowerUpType.Rocket;
        
        public PowerUpBlock Owner { get; set; }
        public GridAxis Orientation { get; set; }
        
        public void Plan(PowerUpResolver resolver)
        {
            if (Owner == null)
            {
                return;
            }
            
            Debug.Log($"Rocket PowerUp with Orientation {Orientation} activated at position {Owner.GridPosition}");

            var gridPosition = Owner.GridPosition;
            var evt = GridEvent.Get(Orientation, gridPosition);
            evt.SendGlobal(channel: (int)GridEventType.RequestAxis);
            
            resolver.EnqueueMany(evt.Blocks, Owner);
            
            evt.Dispose();
        }

        public void Reset()
        {
            Owner = null;
            Orientation = default;
        }
    }
}