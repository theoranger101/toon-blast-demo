using Utilities.Events;

namespace Blocks.EventImplementations
{
    public enum BlockEventType{
        BlockCreated = 0,
    }
    
    public class BlockEvent : Event<BlockEvent>
    {
        public Block Block;

        public static BlockEvent Get(Block block)
        {
            var evt = GetPooledInternal();
            evt.Block = block;
            
            return evt;
        }
    }
}