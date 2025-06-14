using System;
using Blocks.UI;
using Data;
using Object = UnityEngine.Object;

namespace Blocks
{
    public static class BlockFactory
    {
        
        
        public static Block CreateBlock(BlockSpawnData spawnData)
        {
            Block block = spawnData.Category switch
            {
                BlockCategory.Match => new MatchBlock(),
                BlockCategory.PowerUp => new PowerUpBlock(),
                BlockCategory.Obstacle => new ObstacleBlock(),
                _ => throw new Exception("Unsupported block category: " + spawnData.Category)
            };

            block.Init(spawnData);

            // TODO: dummy implementation for testing purposes
            // TODO: Consider using a pooling system instead of instantiating the view
            var blockView = Object.Instantiate(GlobalSettings.Get().MatchBlockPrefab).GetComponent<BlockView>();
            blockView.Init(block);

            return block;
        }
    }
}