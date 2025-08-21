using System.Collections;
using Blocks;
using UnityEngine;
using Utilities.Pooling;

namespace Grid.ClickStrategies
{
    public class MatchBlockClickStrategy : IBlockClickStrategy
    {
        public IEnumerator ResolveClick(GridManager grid, Block block)
        {
            var connected = grid.FindConnectedBlocks(block);

            if (connected.Count <= 1)
            {
                Debug.Log("No connected blocks found to pop at the specified position. Returning");
                yield break;
            }
            
            foreach (var connectedBlock in connected)
            {
                connectedBlock.Pop();
            }

            grid.TriggerRefill(connected);

            ListPool<Block>.Release(connected);
        }
    }
}