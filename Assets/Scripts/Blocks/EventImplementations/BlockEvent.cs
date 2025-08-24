using Blocks.UI;
using Utilities.Events;

namespace Blocks.EventImplementations
{
    public enum BlockEventType{
        BlockCreated = 0,
        BlockClicked = 1,
        BlockPopped = 2,
    }
    
    public class BlockEvent : Event<BlockEvent>
    {
        public Block Block;
        public BlockView BlockView;

        public static BlockEvent Get(Block block)
        {
            var evt = GetPooledInternal();
            evt.Block = block;
            
            return evt;
        }
        
        public static BlockEvent Get(BlockView blockView)
        {
            var evt = GetPooledInternal();
            evt.BlockView = blockView;
            
            return evt;
        }
        
        public override void Dispose()
        {
            base.Dispose();

            Block = null;
            BlockView = null;
        }
    }
}