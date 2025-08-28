using System;
using Blocks.BlockTypes;

namespace Blocks
{
    public static class BlockExtensions
    {
        public static BlockCategory GetCategory(this Block block) => block switch 
        {
            MatchBlock => BlockCategory.Match,
            PowerUpBlock => BlockCategory.PowerUp,
            ObstacleBlock => BlockCategory.Obstacle,
            _ => throw new Exception("Unsupported block category: " + block.GetType())
        };
        
        public static int GetTypeId(this Block b) => b switch
        {
            MatchBlock m => (int) m.Type,
            PowerUpBlock p => (int) p.Type,
            ObstacleBlock o => (int) o.Type,
            _ => -1
        };

    }
}