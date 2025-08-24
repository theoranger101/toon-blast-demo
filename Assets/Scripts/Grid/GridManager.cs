using System;
using System.Collections.Generic;
using Blocks;
using Blocks.EventImplementations;
using Data;
using Grid.ClickStrategies;
using Grid.EventImplementations;
using Grid.Utilities;
using UnityEngine;
using Utilities.Events;
using Utilities.Pooling;

namespace Grid
{
    public enum GridAxis
    {
        Row,
        Column
    }
    
    public class GridManager : MonoBehaviour
    {
        private Block[,] m_Grid;

        // TODO: consider a different approach for global settings
        private GlobalSettings m_Settings;
        
        private void Awake()
        {
            m_Settings = GlobalSettings.Get();
            
            GEM.Subscribe<GridEvent>(HandleGetAxis, channel:(int)GridEventType.RequestAxis);
            GEM.Subscribe<GridEvent>(HandleBlockAdded, channel:(int)GridEventType.AddBlock);
            GEM.Subscribe<GridEvent>(HandleClearPosition, channel:(int)GridEventType.ClearPosition);
            GEM.Subscribe<GridEvent>(HandleBlockMoved, channel:(int)GridEventType.BlockMoved);
            
            GEM.Subscribe<BlockEvent>(HandleBlockPopped, channel:(int)BlockEventType.BlockPopped);
            GEM.Subscribe<BlockEvent>(HandleBlockClicked, channel:(int)BlockEventType.BlockClicked);
        }

        private void OnDestroy()
        {
            GEM.Unsubscribe<GridEvent>(HandleGetAxis, channel:(int)GridEventType.RequestAxis);
            GEM.Unsubscribe<GridEvent>(HandleBlockAdded, channel:(int)GridEventType.AddBlock);
            GEM.Unsubscribe<GridEvent>(HandleClearPosition, channel:(int)GridEventType.ClearPosition);
            GEM.Unsubscribe<GridEvent>(HandleBlockMoved, channel:(int)GridEventType.BlockMoved);
            
            GEM.Unsubscribe<BlockEvent>(HandleBlockPopped, channel:(int)BlockEventType.BlockPopped);
            GEM.Unsubscribe<BlockEvent>(HandleBlockClicked, channel:(int)BlockEventType.BlockClicked);
        }

        #region Event Handlers

        private void HandleBlockPopped(BlockEvent evt)
        {
            RemoveBlock(evt.Block.GridPosition);
        }

        private void HandleBlockClicked(BlockEvent evt)
        {
            OnBlockClicked(evt.Block);
        }

        private void HandleBlockMoved(GridEvent evt)
        {
            SetGridPosition(evt.GridPosition, evt.Block);
        }

        private void HandleClearPosition(GridEvent evt)
        {
            SetGridPosition(evt.GridPosition, null);
        }
        
        private void HandleBlockAdded(GridEvent evt)
        {
            AddBlock(evt.Block, evt.GridPosition);
        }
        
        private void HandleGetAxis(GridEvent evt)
        {
            var result = GetAxis(evt.Axis, evt.GridPosition);
            evt.Blocks = result;
        }

        #endregion
        
        public void InitGrid(Vector2Int gridSize, List<BlockSpawnData> spawnDataList)
        {
            m_Grid = new Block[gridSize.x, gridSize.y];
            
            foreach (var spawnData in spawnDataList)
            {
                var block = BlockFactory.CreateBlock(spawnData);
                var position = spawnData.GridPosition;
                
                AddBlock(block, position);
            }
        }
        
        public void ResetGrid()
        {
            
        }

        public void TriggerRefill(List<Block> blocks)
        {
            using (var refillEvent = GridEvent.Get(blocks))
            {
                refillEvent.SendGlobal(channel: (int)GridEventType.TriggerRefill);
            }
        }
        
        private void OnBlockClicked(Block block)
        {
            IBlockClickStrategy strategy = block switch
            {
                MatchBlock => new MatchBlockClickStrategy(),
                PowerUpBlock => new PowerUpBlockClickStrategy(),
                ObstacleBlock => new ObstacleBlockClickStrategy(),
                _ => throw new NotSupportedException(
                    $"Block type {block.GetType().Name} is not supported for clicking.")
            };
            
            StartCoroutine(strategy.ResolveClick(this, block));
        }
        
        private void SetGridPosition(Vector2Int gridPosition, Block block)
        {
            if (gridPosition.x < 0 || gridPosition.x >= m_Grid.GetLength(0) ||
                gridPosition.y < 0 || gridPosition.y >= m_Grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Grid position is out of bounds.");
            }

            if (block == null)
            {
                m_Grid[gridPosition.x, gridPosition.y] = null;
            }
            else
            {
                m_Grid[gridPosition.x, gridPosition.y] = block;
                block.GridPosition = gridPosition;
            }
        }

        private void AddBlock(Block block, Vector2Int gridPosition)
        {
            Debug.Log("Adding block of type " + block.GetType().Name + " at position " + gridPosition + ".");
            
            if (gridPosition.x < 0 || gridPosition.x >= m_Grid.GetLength(0) ||
                gridPosition.y < 0 || gridPosition.y >= m_Grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Grid position is out of bounds.");
            }

            m_Grid[gridPosition.x, gridPosition.y] = block;
            block.GridPosition = gridPosition;
            // block.OnPopped += HandleBlockPopped;
        }
        
        private void RemoveBlock(Vector2Int gridPosition)
        {
            Debug.Log("Removing block " + " at position " + gridPosition + ".");

            if (gridPosition.x < 0 || gridPosition.x >= m_Grid.GetLength(0) ||
                gridPosition.y < 0 || gridPosition.y >= m_Grid.GetLength(1))
            {
                throw new ArgumentOutOfRangeException(nameof(gridPosition), "Grid position is out of bounds.");
            }
            
            var block = m_Grid[gridPosition.x, gridPosition.y];
            if (block == null) return;

            // block.OnPopped -= HandleBlockPopped;
            m_Grid[gridPosition.x, gridPosition.y] = null;
            
            BlockFactory.ReleaseBlock(block);
        }

        private List<Block> GetAxis(GridAxis axis, Vector2Int gridPosition)
        {
            return axis switch
            {
                GridAxis.Row => GetRow(gridPosition),
                GridAxis.Column => GetColumn(gridPosition),
                _ => throw new ArgumentOutOfRangeException(nameof(axis), "Invalid grid axis specified.")
            };
        }

        private List<Block> GetRow(Vector2Int gridPosition)
        {
            var rowBlocks = ListPool<Block>.Get();
            
            for (var x = 0; x < m_Grid.GetLength(0); x++)
            {
                var block = m_Grid[x, gridPosition.y];
                rowBlocks.Add(block);
            }

            return rowBlocks;
        }

        private List<Block> GetColumn(Vector2Int gridPosition)
        {
            var columnBlocks = ListPool<Block>.Get();
            
            for (var y = 0; y < m_Grid.GetLength(1); y++)
            {
                var block = m_Grid[gridPosition.x, y];
                columnBlocks.Add(block);
            }

            return columnBlocks;
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
        
        #region Editor Utilities

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

        #endregion
    }
}