using System;

namespace Blocks
{
    public class ObstacleBlock : Block
    {
        public ObstacleType Type;
        
        public override bool IsAffectedByGravity { get; }
        public override bool CanBePopped => false;

        public int Strength;

        public override void Init(BlockSpawnData spawnData)
        {
            Type = spawnData.ObstacleType ?? throw new Exception("ObstacleType is required for ObstacleBlock");
            GridPosition = spawnData.GridPosition;
        }

        public override void OnAffectedByPowerUp()
        {
            throw new System.NotImplementedException();
        }

        public override void Pop()
        {
            throw new System.NotImplementedException();
        }
    }
}