using UnityEngine;

namespace Blocks
{
    public struct BlockSpawnData
    {
        public BlockCategory Category;
        public Vector2Int GridPosition;
        
        public MatchBlockType? MatchBlockType;
        public PowerUpType? PowerUpType;
        public ObstacleType? ObstacleType;
    }
}