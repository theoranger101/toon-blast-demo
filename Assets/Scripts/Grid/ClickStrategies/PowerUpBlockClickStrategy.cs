using System.Collections;
using Blocks;

namespace Grid.ClickStrategies
{
    public class PowerUpBlockClickStrategy : IBlockClickStrategy
    {
        public IEnumerator ResolveClick(GridManager grid, Block block)
        {
            using (grid.ResolutionBatch)
            {
                block.Pop();
            }
            
            yield break;
        }
    }
}