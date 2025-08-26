using System;
using System.Collections;
using Blocks;
using Data;
using Grid.EventImplementations;
using UnityEngine;
using Utilities.Events;
using Utilities.Pooling;

namespace Grid.ClickStrategies
{
    public enum PowerUpToCreate
    {
        None = 0,
        Rocket = 1,
        Bomb = 2,
        DiscoBall = 3
    }
    
    public class MatchBlockClickStrategy : IBlockClickStrategy
    {
        private Block m_BlockBeingClicked;
        
        public IEnumerator ResolveClick(GridManager grid, Block block)
        {
            m_BlockBeingClicked = block;
            
            var connected = grid.FindConnectedBlocks(block);
            
            if (connected.Count <= 1)
            {
                Debug.Log("No connected blocks found to pop at the specified position. Returning");
                yield break;
            }
            
            m_BlockBeingClicked.Pop();
            
            var powerUpSpawned = ShouldSpawnPowerUp(connected.Count);
            if (powerUpSpawned)
            {
                connected.Remove(block);
            }
            
            foreach (var connectedBlock in connected)
            {
                connectedBlock.Pop();
            }
            
            grid.TriggerRefill(connected);
            
            ListPool<Block>.Release(connected);
        }

        private bool ShouldSpawnPowerUp(int connectedCount)
        {
            var settings = GlobalSettings.Get();

            var powerUpType = PowerUpToCreate.None;
            if (connectedCount >= settings.MatchCountForDiscoBall)
            {
                powerUpType = PowerUpToCreate.DiscoBall;
            }
            else if (connectedCount >= settings.MatchCountForBomb)
            {
                powerUpType = PowerUpToCreate.Bomb;
            }
            else if (connectedCount >= settings.MatchCountForRocket)
            {
                powerUpType = PowerUpToCreate.Rocket;
            }

            if (powerUpType == PowerUpToCreate.None) return false;
            
            SpawnPowerUpBlock(powerUpType);
            
            return true;
        }

        private void SpawnPowerUpBlock(PowerUpToCreate powerUpType)
        {
            var blockSpawnData = new BlockSpawnData()
            {
                Category = BlockCategory.PowerUp,
                GridPosition = m_BlockBeingClicked.GridPosition
            };
            
            Block spawnedBlock = null;
            
            switch (powerUpType)
            {
                case PowerUpToCreate.Rocket:
                    blockSpawnData.PowerUpType = PowerUpType.Rocket;
                    spawnedBlock = BlockFactory.CreateRocket(blockSpawnData, GridAxis.Row);
                    
                    break;
                case PowerUpToCreate.Bomb:
                    blockSpawnData.PowerUpType = PowerUpType.Bomb;
                    spawnedBlock = BlockFactory.CreateBomb(blockSpawnData);
                    
                    break;
                case PowerUpToCreate.DiscoBall:
                    blockSpawnData.PowerUpType = PowerUpType.DiscoBall;
                    spawnedBlock = BlockFactory.CreateDiscoBall(blockSpawnData, ((MatchBlock)m_BlockBeingClicked).Type);
                    
                    break;
            }

            if (spawnedBlock == null)
            {
                Debug.LogError("(MatchBlockClickStrategy) Failed to spawn block");
            }

            using (var addEvt = GridEvent.Get(spawnedBlock, spawnedBlock.GridPosition))
            {
                addEvt.SendGlobal((int)GridEventType.AddBlock);
            }
        }
    }
}