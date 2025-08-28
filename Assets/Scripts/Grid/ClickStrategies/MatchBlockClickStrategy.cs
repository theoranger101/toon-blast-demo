using System.Collections;
using Blocks;
using Blocks.BlockTypes;
using Data;
using PowerUps;
using UnityEngine;
using Utilities.Pooling;

namespace Grid.ClickStrategies
{
    public class MatchBlockClickStrategy : IBlockClickStrategy
    {
        public IEnumerator ResolveClick(GridManager grid, Block block)
        {
            if (block is not MatchBlock pressed)
            {
                yield break;
            }
            
            var connected = grid.FindConnectedBlocks(pressed);
            
            if (connected.Count <= 1)
            {
                Debug.Log("No connected blocks found to pop at the specified position. Returning");
                ListPool<Block>.Release(connected);
                
                yield break;
            }

            using (grid.ResolutionBatch)
            {
                var settings = GlobalSettings.Get();
                var powerUpPlan = PowerUpRules.Plan(connected.Count, pressed, settings);
                
                if (powerUpPlan.PowerUpToCreate != PowerUpToCreate.None)
                {
                    pressed.Pop();
                    grid.SpawnPowerUp(in powerUpPlan);
                
                    for (var i = 0; i < connected.Count; i++)
                    {
                        var b = connected[i];
                        if (!ReferenceEquals(b, pressed))
                        {
                            b.Pop();
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < connected.Count; i++)
                    {
                        connected[i].Pop();
                    }
                }
            }
            
            ListPool<Block>.Release(connected);
        }
    }
}