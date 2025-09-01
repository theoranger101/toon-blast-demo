using System;
using Blocks;

namespace LevelManagement.Data
{
    public enum ObjectiveType
    {
        RemoveObstacle,
        RemoveColor,
        RemoveBlock,
    }
    
    [Serializable]
    public struct ObjectiveDefinition
    {
        public ObjectiveType ObjectiveType;
        public int Amount;
        
        public MatchBlockType? BlockType;
        public ObstacleType? ObstacleType;
    }
}