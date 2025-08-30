using System.Collections.Generic;
using Blocks;
using DG.Tweening;
using Utilities.Events;

namespace PowerUps.EventImplementations
{
    public enum PowerUpEventType
    {
        PowerUpCreated = 0,
    }
    
    public class PowerUpEvent : Event<PowerUpEvent>
    {
        public Block Block;
        public IReadOnlyList<Block> BlockList;
        public PowerUpToCreate PowerUpToCreate;

        public Tween Tween; 
            
        public static PowerUpEvent Get(Block block, PowerUpToCreate powerUpToCreate, IReadOnlyList<Block> blockList)
        {
            var evt = GetPooledInternal();
            evt.Block = block;
            evt.PowerUpToCreate = powerUpToCreate;
            evt.BlockList = blockList;
            
            return evt;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            Block = null;
            PowerUpToCreate = default;
            BlockList = null;
        }
    }
}