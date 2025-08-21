using Blocks;
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

        public RocketPowerUpStrategy()
        {
            Orientation = Random.Range(0, 2) == 0 ? GridAxis.Row : GridAxis.Column;
        }

        public void Activate()
        {
            Debug.Log("Rocket PowerUp activated at position: " + Owner.GridPosition);

            var gridPosition = Owner.GridPosition;
            var evt = GridEvent.Get(Orientation, gridPosition);
            evt.SendGlobal(channel: (int)GridEventType.RequestAxis);

            var blocksToPop = evt.Blocks;
            
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
            Owner = null;
            
            // TODO: pooling for strategies
        }
    }
}