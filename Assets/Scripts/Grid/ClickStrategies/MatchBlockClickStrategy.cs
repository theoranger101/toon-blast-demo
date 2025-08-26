using System.Collections;
using Blocks;
using Data;
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
            
            block.Pop();

            var powerUpPlan = PowerUpRules.Plan(connected.Count, (MatchBlock)block, GlobalSettings.Get());
            if (powerUpPlan.PowerUpToCreate != PowerUpToCreate.None)
            {
                grid.SpawnPowerUp(powerUpPlan);
                connected.Remove(block);
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