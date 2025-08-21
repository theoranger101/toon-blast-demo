using System;
using Blocks.UI;
using Data;
using Utilities.Pooling;
using Object = UnityEngine.Object;

namespace Blocks
{
    public static class BlockFactory
    {
        private static GenericPool<MatchBlock> s_MatchBlockPool = new();
        private static GenericPool<PowerUpBlock> s_PowerUpBlockPool = new();
        private static GenericPool<ObstacleBlock> s_ObstacleBlockPool = new();
        
        public static Block CreateBlock(BlockSpawnData spawnData)
        {
            Block block = spawnData.Category switch
            {
                BlockCategory.Match => s_MatchBlockPool.Get(),
                BlockCategory.PowerUp => s_PowerUpBlockPool.Get(),
                BlockCategory.Obstacle => s_ObstacleBlockPool.Get(),
                _ => throw new Exception("Unsupported block category: " + spawnData.Category)
            };

            block.Init(spawnData);

            // TODO: dummy implementation for testing purposes
            // TODO: Consider using a pooling system instead of instantiating the view
            var blockView = Object.Instantiate(GlobalSettings.Get().MatchBlockPrefab).GetComponent<BlockView>();
            blockView.Init(block);

            return block;
        }

        public static void ReleaseBlock(Block block)
        {
            switch (block)
            {
                case MatchBlock matchBlock:
                    s_MatchBlockPool.Release(matchBlock);
                    break;
                case PowerUpBlock powerUpBlock:
                    s_PowerUpBlockPool.Release(powerUpBlock);
                    break;
                case ObstacleBlock obstacleBlock:
                    s_ObstacleBlockPool.Release(obstacleBlock);
                    break;
            }
        }
    }
}