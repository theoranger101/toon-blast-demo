using System;
using System.Collections.Generic;
using Blocks;
using Blocks.UI;
using Data;
using Grid.Utilities;
using UnityEngine;
using Utilities.Pooling;
using Random = UnityEngine.Random;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        private Block[,] m_Grid;

        // TODO: consider a different approach for global settings
        private GlobalSettings m_Settings;

        public static event Action<Block> OnBlockMoved;
        
        private void Awake()
        {
            m_Settings = GlobalSettings.Get();
        }

        public void InitGrid(Vector2Int gridSize, List<BlockSpawnData> spawnDataList)
        {
            m_Grid = new Block[gridSize.x, gridSize.y];
            
            foreach (var spawnData in spawnDataList)
            {
                Block block = BlockFactory.CreateBlock(spawnData);
                Vector2Int position = spawnData.GridPosition;
                
                if (position.x >= 0 && position.x < gridSize.x && position.y >= 0 && position.y < gridSize.y)
                {
                    m_Grid[position.x, position.y] = block;
                }
            }

            BlockView.OnBlockClicked += OnBlockClicked;
        }

        public void PrintGrid()
        {
            for (var y = 0; y < m_Grid.GetLength(1); y++)
            {
                for (var x = 0; x < m_Grid.GetLength(0); x++)
                {
                    if (m_Grid[x, y] != null)
                    {
                        Debug.Log($"Block at ({x}, {y}): {m_Grid[x, y].GetType().Name}");
                    }
                    else
                    {
                        Debug.Log($"Block at ({x}, {y}): Empty");
                    }
                }
            }
        }

        public void PrintAvailableMatches()
        {
            for (var y = 0; y < m_Grid.GetLength(1); y++)
            {
                for (var x = 0; x < m_Grid.GetLength(0); x++)
                {
                    Block block = m_Grid[x, y];
                    
                    if (block is not MatchBlock matchBlock) continue;
                    
                    var connectedBlocks = FindConnectedBlocks(block);
                    if (connectedBlocks.Count >= 1)
                    {
                        Debug.Log(
                            $"Match found at ({x}, {y}): {connectedBlocks.Count} connected blocks of type {matchBlock.Type}");
                    }

                    if (connectedBlocks.Count >= m_Settings.MatchCountForDiscoBall)
                    {
                        
                    }
                    else if (connectedBlocks.Count >= m_Settings.MatchCountForBomb)
                    {
                        
                    }
                    else if (connectedBlocks.Count >= m_Settings.MatchCountForRocket)
                    {
                        
                    }
                        
                    ListPool<Block>.Release(connectedBlocks);
                }
            }
        }

        public void ResetGrid()
        {
            
        }
        
        private void OnBlockClicked(Block block)
        {
            if (block == null)
            {
                Debug.LogWarning("Block is null in OnBlockClicked.");
                return;
            }
            
            TryPopAt(block.GridPosition);
        }
        
        public void TryPopAt(Vector2Int gridPos)
        {
            var poppedBlock = m_Grid[gridPos.x, gridPos.y];
            
            if (poppedBlock != null)
            {
                var connectedBlocks = FindConnectedBlocks(poppedBlock);
                
                if (connectedBlocks.Count <= 1)
                {
                    Debug.Log("No connected blocks found to pop at the specified position. Returning");
                    return;
                }
                
                foreach (var connectedBlock in connectedBlocks)
                {
                    m_Grid[connectedBlock.GridPosition.x, connectedBlock.GridPosition.y] = null;
                    connectedBlock.Pop();
                }
                
                TriggerRefill(connectedBlocks);
                
                ListPool<Block>.Release(connectedBlocks);
            }
            else
            {
                throw new Exception("No block found at the specified grid position.");
            }
        }

        public void TriggerRefill(List<Block> emptiedBlocks)
        {
            var columnsToRefill = HashSetPool<int>.Get();

            for (var i = 0; i < emptiedBlocks.Count; i++)
            {
                columnsToRefill.Add(emptiedBlocks[i].GridPosition.x);
            }
            
            foreach (var column in columnsToRefill)
            {
                RefillColumn(column);
            }
        }

        private void RefillColumn(int columnIndex)
        {
            var targetY = 0; // target position to move a block to

            for (var scanY = 0; scanY < m_Grid.GetLength(1); scanY++)
            {
                var block = m_Grid[columnIndex, scanY];
                
                if (block == null)
                {
                    continue;
                }

                if (!block.IsAffectedByGravity)
                {
                    targetY = scanY + 1;
                    continue;
                }

                if (scanY != targetY)
                {
                    m_Grid[columnIndex, targetY] = block;
                    m_Grid[columnIndex, scanY] = null;

                    block.GridPosition = new Vector2Int(columnIndex, targetY);
                    
                    OnBlockMoved?.Invoke(block);
                }

                targetY++;
            }
            
            // Fill the remaining empty spaces in the column with new blocks
            for (var fillY = targetY; fillY < m_Grid.GetLength(1); fillY++)
            {
                var randomSpawnData = new BlockSpawnData
                {
                    Category = BlockCategory.Match,
                    MatchBlockType = (MatchBlockType) Random.Range(0, 4),
                    GridPosition = new Vector2Int(columnIndex, fillY)
                };
                
                var newBlock = BlockFactory.CreateBlock(randomSpawnData);
                m_Grid[columnIndex, fillY] = newBlock;
                newBlock.GridPosition = new Vector2Int(columnIndex, fillY);
            }
        }
        
        public List<Block> FindConnectedBlocks(Block startBlock)
        {
            IMatchDetectionStrategy strategy = startBlock switch
            {
                MatchBlock match => new GenericMatchDetectionStrategy(),
                _ => throw new Exception("Unsupported block category for match detection. Only Match blocks are supported.")
            };

            return strategy.FindConnectedMatches(startBlock, m_Grid);
        }
    }
}