using System.Collections;
using Blocks;
using PowerUps;

namespace Grid.ClickStrategies
{
    public class PowerUpBlockClickStrategy : IBlockClickStrategy
    {
        public IEnumerator ResolveClick(GridManager grid, Block block)
        {
            using (var resolver = new PowerUpResolver(grid))
            {
                resolver.SeedFromClick(block);
                resolver.PlanAll();
                
                resolver.Apply();
            }
            
            yield break;
        }
    }
}