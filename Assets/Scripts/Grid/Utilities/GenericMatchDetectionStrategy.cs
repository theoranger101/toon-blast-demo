using System.Collections.Generic;
using Blocks;
using Blocks.BlockTypes;
using UnityEngine;
using Utilities.Pooling;

namespace Grid.Utilities
{
    public class GenericMatchDetectionStrategy : IMatchDetectionStrategy
    {
        public List<Block> FindConnectedMatches(Block startBlock, Block[,] grid)
        {
            if (startBlock == null || grid == null || grid.GetLength(0) == 0 || grid.GetLength(1) == 0
                || startBlock is not MatchBlock matchBlock)
            {
                return ListPool<Block>.Get();
            }

            var connectedBlocks = ListPool<Block>.Get();
            var visited = HashSetPool<Block>.Get();

            DepthFirstSearch(matchBlock);

            HashSetPool<Block>.Release(visited);

            return connectedBlocks;
            
            void DepthFirstSearch(Block block)
            {
                if (block == null || visited.Contains(block) || block is not MatchBlock match || match.Type != matchBlock.Type)
                    return;

                visited.Add(block);
                connectedBlocks.Add(block);
                
                Debug.Log("Visiting block at position: " + block.GridPosition + " of type: " + match.Type + "");

                // Check adjacent blocks (up, down, left, right)
                var x = block.GridPosition.x;
                var y = block.GridPosition.y;

                if (x > 0) DepthFirstSearch(grid[x - 1, y]); // Left
                if (x < grid.GetLength(0) - 1) DepthFirstSearch(grid[x + 1, y]); // Right
                if (y > 0) DepthFirstSearch(grid[x, y - 1]); // Down
                if (y < grid.GetLength(1) - 1) DepthFirstSearch(grid[x, y + 1]); // Up
            }
        }
    }
}